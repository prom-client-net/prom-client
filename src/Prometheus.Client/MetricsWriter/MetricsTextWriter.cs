using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Prometheus.Client.MetricsWriter
{
    internal class MetricsTextWriter: IMetricsWriter, ISampleWriter, ILabelWriter, IDisposable
    {
        [Flags]
        private enum WriterState
        {
            Empty,
            MetricStarted,
            HelpWritten,
            TypeWritten,
            SampleStarted,
            LabelsStarted,
            LabelWritten,
            LabelsClosed,
            ValueWritten,
            TimestampWritten,
            Closed,
        }

        private static readonly Encoding _encoding = new UTF8Encoding(false);
        private static readonly IReadOnlyDictionary<MetricType, string> _metricTypesMap = BuildMetricTypesMap();

        private const string _commentPrefix = "#";
        private const string _tokenSeparator = " ";
        private const string _helpPrefix = "HELP";
        private const string _typePrefix = "TYPE";
        private const string _labelsStart = "{";
        private const string _labelsEnd = "}";
        private const string _labelsEq = "=";
        private const string _labelsSeparator = ",";
        private const string _labelTextQualifier = "\"";

        private readonly StreamWriter _streamWriter;
        private WriterState _state = WriterState.Empty;
        private string _currentMetricName;
        private bool _hasData = false;

        public MetricsTextWriter(Stream stream)
        {
            _streamWriter = new StreamWriter(stream, _encoding, bufferSize: 1024, leaveOpen: true);
            _streamWriter.NewLine = "\n";
        }

        public IMetricsWriter StartMetric(string metricName)
        {
            ValidateState(nameof(StartMetric), WriterState.Empty | WriterState.ValueWritten | WriterState.TimestampWritten);

            _currentMetricName = metricName;
            _state = WriterState.MetricStarted;

            return this;
        }

        public IMetricsWriter WriteHelp(string help)
        {
            ValidateState(nameof(WriteHelp), WriterState.MetricStarted);

            if (_hasData)
            {
                _streamWriter.WriteLine();
            }
            _hasData = true;
            _streamWriter.Write(_commentPrefix);
            _streamWriter.Write(_tokenSeparator);
            _streamWriter.Write(_helpPrefix);
            _streamWriter.Write(_tokenSeparator);
            _streamWriter.Write(_currentMetricName);
            _streamWriter.Write(_tokenSeparator);
            _streamWriter.Write(EscapeValue(help));
            _state = WriterState.HelpWritten;

            return this;
        }

        public IMetricsWriter WriteType(MetricType metricType)
        {
            ValidateState(nameof(WriteType), WriterState.MetricStarted | WriterState.HelpWritten);

            if (_hasData)
            {
                _streamWriter.WriteLine();
            }
            _hasData = true;
            _streamWriter.Write(_commentPrefix);
            _streamWriter.Write(_tokenSeparator);
            _streamWriter.Write(_typePrefix);
            _streamWriter.Write(_tokenSeparator);
            _streamWriter.Write(_currentMetricName);
            _streamWriter.Write(_tokenSeparator);
            _streamWriter.Write(_metricTypesMap[metricType]);
            _state = WriterState.TypeWritten;

            return this;
        }

        public ISampleWriter StartSample(string suffix = "")
        {
            ValidateState(nameof(StartSample), WriterState.MetricStarted | WriterState.HelpWritten | WriterState.TypeWritten | WriterState.ValueWritten | WriterState.TimestampWritten);

            if (_hasData)
            {
                _streamWriter.WriteLine();
            }
            _hasData = true;
            _streamWriter.Write(_currentMetricName);
            _streamWriter.Write(suffix);
            _state = WriterState.SampleStarted;
            return this;
        }

        public void CloseWriter()
        {
            ValidateState(nameof(CloseWriter), WriterState.Empty | WriterState.ValueWritten | WriterState.TimestampWritten);

            _streamWriter.WriteLine();
            _streamWriter.Flush();
            _state = WriterState.Closed;
        }

        public ILabelWriter StartLabels()
        {
            ValidateState(nameof(StartLabels), WriterState.SampleStarted);
            _streamWriter.Write(_labelsStart);
            _state = WriterState.LabelsStarted;

            return this;
        }

        public ISampleWriter WriteValue(double value)
        {
            ValidateState(nameof(WriteLabel), WriterState.SampleStarted | WriterState.LabelsClosed);
            _streamWriter.Write(_tokenSeparator);
            _streamWriter.Write(value.ToString(CultureInfo.InvariantCulture));
            _state = WriterState.ValueWritten;

            return this;
        }

        public ISampleWriter WriteTimestamp(long timestamp)
        {
            ValidateState(nameof(WriteLabel), WriterState.ValueWritten);
            _streamWriter.Write(_tokenSeparator);
            _streamWriter.Write(timestamp.ToString(CultureInfo.InvariantCulture));
            _state = WriterState.TimestampWritten;

            return this;
        }

        public IMetricsWriter EndSample()
        {
            return this;
        }

        public ILabelWriter WriteLabel(string name, string value)
        {
            ValidateState(nameof(WriteLabel), WriterState.LabelsStarted | WriterState.LabelWritten);
            if (_state == WriterState.LabelWritten)
            {
                _streamWriter.Write(_labelsSeparator);
            }
            _streamWriter.Write(name);
            _streamWriter.Write(_labelsEq);
            _streamWriter.Write(_labelTextQualifier);
            _streamWriter.Write(EscapeValue(value));
            _streamWriter.Write(_labelTextQualifier);

            _state = WriterState.LabelWritten;
            return this;
        }

        public ISampleWriter EndLabels()
        {
            ValidateState(nameof(WriteLabel), WriterState.LabelWritten);
            _streamWriter.Write(_labelsEnd);
            _state = WriterState.LabelsClosed;
            return this;
        }

        private static IReadOnlyDictionary<MetricType, string> BuildMetricTypesMap()
        {
            var values = (MetricType[])Enum.GetValues(typeof(MetricType));

            return values.ToDictionary(k => k, v => Enum.GetName(typeof(MetricType), v).ToLowerInvariant());
        }

        private void ValidateState(string callerMethod, WriterState expectedStates)
        {
            if ((_state & expectedStates) != _state)
            {
                throw new InvalidOperationException($"Cannot {callerMethod}. Current state: {_state}, expected states: {expectedStates}");
            }
        }

        private string EscapeValue(string val)
        {
            return val
                .Replace("\\", @"\\")
                .Replace("\n", @"\n")
                .Replace("\"", @"\""");
        }

        public void Dispose()
        {
            CloseWriter();
            _streamWriter.Dispose();
        }
    }
}
