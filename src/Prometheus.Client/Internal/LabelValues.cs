using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prometheus.Contracts;

namespace Prometheus.Client.Internal
{
    public class LabelValues : IEquatable<LabelValues>
    {
        public static readonly LabelValues Empty = new LabelValues(new string[0], new string[0]);
        private readonly string[] _values;
        internal readonly List<LabelPair> WireLabels = new List<LabelPair>();

        public LabelValues(IReadOnlyCollection<string> names, IReadOnlyList<string> values)
        {
            if (values == null)
                throw new InvalidOperationException("Label values is null");

            if (names.Count != values.Count)
                throw new InvalidOperationException("Label values must be of same length as label names");

            _values = new string[values.Count];
            for (var i = 0; i < values.Count; i++)
                _values[i] = values[i] ?? "";

            WireLabels.AddRange(names.Zip(values, (s, s1) => new LabelPair {name = s, value = s1}));
        }

        public bool Equals(LabelValues labelValues)
        {
            if (labelValues?._values.Length != _values.Length)
                return false;

            return !_values.Where((t, i) => t != labelValues._values[i]).Any();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is LabelValues labelValues))
                return false;

            return Equals(labelValues);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _values.Aggregate(1, (current, val) => current ^ (val.GetHashCode() * 397));
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var label in WireLabels)
            {
                sb.AppendFormat("{0}={1}, ", label.name, label.value);
            }

            return sb.ToString();
        }
    }
}