using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
#if NETCORE
using System.Buffers.Text;
#else
using System.Globalization;
#endif

namespace Prometheus.Client.MetricsWriter
{
    internal sealed class MetricsTextWriter : IMetricsWriter, ISampleWriter, ILabelWriter
    {
        private static readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;
#if NETCORE
        private static readonly ArrayPool<char> _charPool = ArrayPool<char>.Shared;
#endif
        private static readonly Encoding _encoding = new UTF8Encoding(false);

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
        private byte[] _buffer;
        private int _position;
        private string _currentMetricName;
        private bool _hasData;
        private WriterState _state = WriterState.None;

        public MetricsTextWriter(Stream stream)
        {
            _stream = stream;
            _buffer = _arrayPool.Rent(1024);
            Write(_encoding.GetPreamble());
        }

        // Should use finalizer to ensure _buffer returned into pool.
        ~MetricsTextWriter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
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
            ValidateState(nameof(WriteLabel), WriterState.SampleStarted | WriterState.LabelsClosed);
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
            ValidateState(nameof(EndMetric), WriterState.SampleClosed);
            _currentMetricName = string.Empty;
            _state = WriterState.MetricClosed;
            return this;
        }

        public void Close()
        {
            Write(_newLine);
            Flush();
            _state = WriterState.Closed;
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
                EnsureBufferCapacity(buff.Length);

                value.TryFormat(buff, out var charsize);
                var size = _encoding.GetBytes(buff, 0, charsize, _buffer, _position);
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

            if (size > _buffer.Length)
            {
                var buff = _arrayPool.Rent(size);
                try
                {
                    _encoding.GetBytes(value, 0, value.Length, buff, 0);
                    _stream.Write(buff, 0, size);
                }
                finally
                {
                    _arrayPool.Return(buff);
                }
            }
            else
            {
                _encoding.GetBytes(value, 0, value.Length, _buffer, _position);
                _position += size;
            }
        }

        private void Write(byte[] bytes)
        {
            Write(bytes, bytes.Length);
        }

        private void Write(byte[] bytes, int len)
        {
            EnsureBufferCapacity(len);

            if (len > _buffer.Length)
            {
                _stream.Write(bytes, 0, len);
            }
            else
            {
                Array.Copy(bytes, 0, _buffer, _position, len);
                _position += len;
            }
        }

        private void Dispose(bool disposing)
        {
            if (_buffer == null)
                return;

            if (disposing && _state != WriterState.Closed)
                Close();

            _arrayPool.Return(_buffer);
            _buffer = null;
        }

        // Ensure if current buffer has neccesary space available for next write
        // if there is not enough space available - flush it
        private void EnsureBufferCapacity(int requiredCapacity)
        {
            if ((_buffer.Length - _position) < requiredCapacity)
                Flush();
        }

        private void Flush()
        {
            if (_position == 0)
                return;

            _stream.Write(_buffer, 0, _position);
            _position = 0;
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
