using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Prometheus.Client.SummaryImpl;
// Ported from https://github.com/beorn7/perks/blob/master/quantile/stream.go

// Package quantile computes approximate quantiles over an unbounded data
// stream within low memory and CPU bounds.
//
// A small amount of accuracy is traded to achieve the above properties.
//
// Multiple streams can be merged before calling Query to generate a single set
// of results. This is meaningful when the streams represent the same type of
// data. See Merge and Samples.
//
// For more detailed information about the algorithm used, see:
//
// Effective Computation of Biased Quantiles over Data Streams
//
// http://www.cs.rutgers.edu/~muthu/bquant.pdf
internal sealed class QuantileStream
{
    private readonly object _bufferLock = new object();
    private readonly TimeSpan _streamDuration;
    private readonly ReaderWriterLockSlim _sampleStreamsLock = new ReaderWriterLockSlim();
    private readonly double[] _buffer;
    private readonly SampleStream[] _sampleStreams;
    private readonly Func<DateTimeOffset> _currentTimeProvider;

    private volatile int _bufferPosition;
    private int _headStreamIndex;
    private DateTimeOffset _nextStreamRotationOffset;

    public QuantileStream(int bufferSize, TimeSpan streamDuration, int ageBuckets, Invariant invariant, Func<DateTimeOffset> currentTimeProvider = null)
    {
        _buffer = new double[bufferSize];
        _streamDuration = streamDuration;
        _sampleStreams = new SampleStream[ageBuckets];
        for(var i = 0; i < ageBuckets; i++)
            _sampleStreams[i] = new SampleStream(invariant);

        _currentTimeProvider = currentTimeProvider ?? (() => DateTimeOffset.UtcNow);
        _nextStreamRotationOffset = _currentTimeProvider().Add(_streamDuration);
    }

    public void Append(double value)
    {
        while (true)
        {
            if(ShouldFlushBuffer())
                FlushBuffer();

            lock (_bufferLock)
            {
                // use this trick to make FlushBuffer outside of the lock
                if (ShouldFlushBuffer())
                    continue;

                _buffer[_bufferPosition++] = value;
                return;
            }
        }
    }

    public void Reset()
    {
        lock (_bufferLock)
        {
            _bufferPosition = 0;
            _headStreamIndex = 0;
        }

        _sampleStreamsLock.EnterWriteLock();
        try
        {
            for(var i = 0; i < _sampleStreams.Length; i++)
                _sampleStreams[i].Reset();
        }
        finally
        {
            _sampleStreamsLock.ExitWriteLock();
        }
    }

    public void FlushBuffer()
    {
        Span<double> data = stackalloc double[_buffer.Length];
        FlushBuffer(data, out int bufferSize);
        data = data.Slice(0, bufferSize);

        PopulateSampleStreams(data);
    }

    private void FlushBuffer(Span<double> destination, out int bufferSize)
    {
        bufferSize = 0;
        if (_bufferPosition == 0)
            return;

        lock (_bufferLock)
        {
            if (_bufferPosition == 0)
                return;

            Array.Sort(_buffer, 0, _bufferPosition);

            var data = new Span<double>(_buffer);
            data = data.Slice(0, _bufferPosition);
            data.CopyTo(destination);

            bufferSize = _bufferPosition;
            _bufferPosition = 0;
        }
    }

    private void PopulateSampleStreams(ReadOnlySpan<double> data)
    {
        var now = _currentTimeProvider();
        if (data.IsEmpty && now < _nextStreamRotationOffset)
            return;

        _sampleStreamsLock.EnterWriteLock();
        try
        {
            while (now >= _nextStreamRotationOffset)
            {
                _sampleStreams[_headStreamIndex].Reset();
                _headStreamIndex++;

                if (_headStreamIndex >= _sampleStreams.Length)
                    _headStreamIndex = 0;

                _nextStreamRotationOffset = _nextStreamRotationOffset.Add(_streamDuration);
            }

            if (!data.IsEmpty)
            {
                foreach (var sampleStream in _sampleStreams)
                    sampleStream.InsertRange(data);
            }
        }
        finally
        {
            _sampleStreamsLock.ExitWriteLock();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ShouldFlushBuffer()
    {
        if (_bufferPosition >= _buffer.Length)
            return true;

        if (_currentTimeProvider() > _nextStreamRotationOffset)
            return true;

        return false;
    }

    // Query returns the computed qth percentiles value. If s was created with
    // NewTargeted, and q is not in the set of quantiles provided a priori, Query
    // will return an unspecified result.
    public double Query(double q)
    {
        _sampleStreamsLock.EnterReadLock();

        try
        {
            return _sampleStreams[_headStreamIndex].Query(q);
        }
        finally
        {
            _sampleStreamsLock.ExitReadLock();
        }
    }
}
