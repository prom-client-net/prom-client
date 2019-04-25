using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Prometheus.Client.MetricsWriter.Abstractions;
#if NETCORE
using System.Buffers.Text;
#else
using System.Globalization;
#endif

namespace Prometheus.Client.MetricsWriter
{
    internal sealed class MetricsTextWriter : IMetricsWriter, ISampleWriter, ILabelWriter
    {
        private const int _defaultBufferSize = 10240;
        private static readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Create(_defaultBufferSize, 100);
#if NETCORE
        private static readonly ArrayPool<char> _charPool = ArrayPool<char>.Shared;
#endif
        private static readonly Encoding _encoding = new UTF8Encoding(false);

        private static readonly byte[] _encodingPreamble = _encoding.GetPreamble();
        private static readonly byte[] _commentPrefix = _encoding.GetBytes("#");
        private static readonly byte[] _tokenSeparator = _encoding.GetBytes(" ");
        private static readonly byte[] _helpPrefix = _encoding.GetBytes("HELP");
        private static readonly byte[] _typePrefix = _encoding.GetBytes("TYPE");
        private static readonly byte[] _labelsStart = _encoding.GetBytes("{");
        private static readonly byte[] _labelsEnd = _encoding.GetBytes("}");
        private static readonly byte[] _labelsEq = _encoding.GetBytes("=");
        private static readonly byte[] _labelsSeparator = _encoding.GetBytes(",");
        private static readonly byte[] _labelTextQualifier = _encoding.GetBytes("\"");
        private static readonly byte[] _newLine = _encoding.GetBytes("\n");
        private static readonly IReadOnlyDictionary<MetricType, byte[]> _metricTypesMap = BuildMetricTypesMap();

        private readonly Stream _stream;
        private readonly Queue<ArraySegment<byte>> _chunks;
        private byte[] _buffer;
        private int _position;
        private string _currentMetricName;
        private bool _hasData;
        private WriterState _state = WriterState.None;

        public MetricsTextWriter(Stream stream)
        {
            _stream = stream;
            _buffer = _arrayPool.Rent(_defaultBufferSize);
            _chunks = new Queue<ArraySegment<byte>>();
            Write(_encodingPreamble);
        }

        // Should use finalizer to ensure _buffer returned into pool.
        ~MetricsTextWriter()
        {
            CleanUp();
        }

        public void Dispose()
        {
            CleanUp();
            GC.SuppressFinalize(this);
        }

        public IMetricsWriter StartMetric(string metricName)
        {
            ValidateState(nameof(StartMetric), WriterState.None | WriterState.MetricClosed);

            _currentMetricName = metricName;
            _state = WriterState.MetricStarted;

            return this;
        }

        public IMetricsWriter WriteHelp(string help)
        {
            ValidateState(nameof(WriteHelp), WriterState.MetricStarted);

            if (_hasData)
                Write(_newLine);

            _hasData = true;
            Write(_commentPrefix);
            Write(_tokenSeparator);
            Write(_helpPrefix);
            Write(_tokenSeparator);
            Write(_currentMetricName);
            Write(_tokenSeparator);
            Write(EscapeValue(help));
            _state = WriterState.HelpWritten;

            return this;
        }

        public IMetricsWriter WriteType(MetricType metricType)
        {
            ValidateState(nameof(WriteType), WriterState.MetricStarted | WriterState.HelpWritten);

            if (_hasData)
                Write(_newLine);

            _hasData = true;
            Write(_commentPrefix);
            Write(_tokenSeparator);
            Write(_typePrefix);
            Write(_tokenSeparator);
            Write(_currentMetricName);
            Write(_tokenSeparator);
            Write(_metricTypesMap[metricType]);
            _state = WriterState.TypeWritten;

            return this;
        }

        public ISampleWriter StartSample(string suffix = "")
        {
            ValidateState(nameof(StartSample),
                WriterState.MetricStarted | WriterState.HelpWritten | WriterState.TypeWritten | WriterState.SampleClosed);

            if (_hasData)
                Write(_newLine);

            _hasData = true;
            Write(_currentMetricName);
            Write(suffix);
            _state = WriterState.SampleStarted;
            return this;
        }

        public ILabelWriter StartLabels()
        {
            ValidateState(nameof(StartLabels), WriterState.SampleStarted);
            Write(_labelsStart);
            _state = WriterState.LabelsStarted;

            return this;
        }

        public ILabelWriter WriteLabel(string name, string value)
        {
            ValidateState(nameof(WriteLabel), WriterState.LabelsStarted | WriterState.LabelWritten);
            if (_state == WriterState.LabelWritten)
                Write(_labelsSeparator);

            Write(name);
            Write(_labelsEq);
            Write(_labelTextQualifier);
            Write(EscapeValue(value));
            Write(_labelTextQualifier);

            _state = WriterState.LabelWritten;
            return this;
        }

        public ISampleWriter EndLabels()
        {
            ValidateState(nameof(EndLabels), WriterState.LabelWritten);
            Write(_labelsEnd);
            _state = WriterState.LabelsClosed;
            return this;
        }

        public ISampleWriter WriteValue(double value)
        {
            ValidateState(nameof(WriteValue), WriterState.SampleStarted | WriterState.LabelsClosed);
            Write(_tokenSeparator);
            Write(value);
            _state = WriterState.ValueWritten;

            return this;
        }

        public ISampleWriter WriteTimestamp(long timestamp)
        {
            ValidateState(nameof(WriteTimestamp), WriterState.ValueWritten);
            Write(_tokenSeparator);
            Write(timestamp);
            _state = WriterState.TimestampWritten;

            return this;
        }

        public IMetricsWriter EndSample()
        {
            ValidateState(nameof(EndSample), WriterState.ValueWritten | WriterState.TimestampWritten);
            _state = WriterState.SampleClosed;
            return this;
        }

        public IMetricsWriter EndMetric()
        {
            ValidateState(nameof(EndMetric), WriterState.SampleClosed | WriterState.MetricStarted | WriterState.TypeWritten | WriterState.HelpWritten);
            _currentMetricName = string.Empty;
            _state = WriterState.MetricClosed;
            return this;
        }

        public Task CloseWriterAsync()
        {
            Write(_newLine);
            _state = WriterState.Closed;

            return FlushInternalAsync(true);
        }

        public Task FlushAsync()
        {
            if (_chunks.Count == 0 && _position == 0)
            {
                return Task.CompletedTask;
            }

            return FlushInternalAsync(false);
        }

        private async Task FlushInternalAsync(bool freeUpCurrentBuffer)
        {
            while (_chunks.Count > 0)
            {
                var chunk = _chunks.Dequeue();
                await _stream.WriteAsync(chunk.Array, chunk.Offset, chunk.Count).ConfigureAwait(false);
                _arrayPool.Return(chunk.Array);
            }

            await _stream.WriteAsync(_buffer, 0, _position).ConfigureAwait(false);
            if (freeUpCurrentBuffer)
                _arrayPool.Return(_buffer);

            _position = 0;
        }

        private static IReadOnlyDictionary<MetricType, byte[]> BuildMetricTypesMap()
        {
            var values = (MetricType[])Enum.GetValues(typeof(MetricType));

            return values.ToDictionary(k => k, v => _encoding.GetBytes(Enum.GetName(typeof(MetricType), v).ToLowerInvariant()));
        }

        private void ValidateState(string callerMethod, WriterState expectedStates)
        {
            if ((_state & expectedStates) != _state)
                throw new InvalidOperationException($"Cannot {callerMethod}. Current state: {_state}, expected states: {expectedStates}");
        }

        private string EscapeValue(string val)
        {
            return val
                   .Replace("\\", @"\\")
                   .Replace("\n", @"\n")
                   .Replace("\"", @"\""");
        }

        private void Write(double value)
        {
#if NETCORE
            var buff = _charPool.Rent(32);
            try
            {
                value.TryFormat(buff, out var charsize);
                var size = _encoding.GetByteCount(buff, 0, charsize);

                EnsureBufferCapacity(size);
                _encoding.GetBytes(buff, 0, charsize, _buffer, _position);
                _position += size;
            }
            finally
            {
                _charPool.Return(buff);
            }
#else
            Write(value.ToString(CultureInfo.InvariantCulture));
#endif
        }

        private void Write(string value)
        {
            var size = _encoding.GetByteCount(value);
            EnsureBufferCapacity(size);

            _encoding.GetBytes(value, 0, value.Length, _buffer, _position);
            _position += size;
        }

        private void Write(byte[] bytes)
        {
            Write(bytes, bytes.Length);
        }

        private void Write(byte[] bytes, int len)
        {
            EnsureBufferCapacity(len);

            Array.Copy(bytes, 0, _buffer, _position, len);
            _position += len;
        }

        private void CleanUp()
        {
            if (_buffer != null)
            {
                _arrayPool.Return(_buffer);
                _buffer = null;
            }

            while (_chunks.Count > 0)
            {
                var chunk = _chunks.Dequeue();
                _arrayPool.Return(chunk.Array);
            }
        }

        // Ensure if current buffer has neccesary space available for next write
        // if there is not enough space available - rotate the buffers
        private void EnsureBufferCapacity(int requiredCapacity)
        {
            if ((_buffer.Length - _position) < requiredCapacity)
            {
                _chunks.Enqueue(new ArraySegment<byte>(_buffer, 0, _position));
                _buffer = _arrayPool.Rent(Math.Max(requiredCapacity, _defaultBufferSize));
                _position = 0;
            }
        }

        [Flags]
        private enum WriterState
        {
            None = 0,
            MetricStarted = 1,
            HelpWritten = 2,
            TypeWritten = 4,
            SampleStarted = 8,
            LabelsStarted = 16,
            LabelWritten = 32,
            LabelsClosed = 64,
            ValueWritten = 128,
            TimestampWritten = 256,
            SampleClosed = 512,
            MetricClosed = 1024,
            Closed = 2048
        }
    }
}
