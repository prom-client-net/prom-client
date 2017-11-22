using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prometheus.Contracts;

namespace Prometheus.Client.Internal
{
    internal struct LabelValues : IEquatable<LabelValues>
    {
        private readonly int _hashCode;
        private readonly string[] _values;

        internal static readonly LabelValues Empty = new LabelValues(new string[0], new string[0]);
        internal readonly List<LabelPair> WireLabels;

        public LabelValues(string[] names, string[] values)
        {
            if (values == null)
                throw new InvalidOperationException("Label values is null");

            if (names.Length != values.Length)
                throw new InvalidOperationException("Label values must be of same length as label names");

            _values = values;

            for (var i = 0; i < values.Length; i++)
                _values[i] = values[i] ?? "";

            WireLabels = new List<LabelPair>(names.Zip(values, (s, s1) => new LabelPair {name = s, value = s1}));

            // Calculating the hash code is fast but we don't need to re-calculate it for each comparison this object is involved in.
            // Label values are fixed- caluclate it once up-front and remember the value.
            _hashCode = CalculateHashCode(_values);
        }

        public bool Equals(LabelValues labelValues)
        {
            if (labelValues._values.Length != _values.Length)
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var label in WireLabels)
            {
                sb.AppendFormat("{0}={1}, ", label.name, label.value);
            }

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        private static int CalculateHashCode(string[] values)
        {
            unchecked
            {
                var hashCode = 1;
                for (var i = 0; i < values.Length; i++)
                {
                    hashCode ^= values[i].GetHashCode() * 397;
                }
                return hashCode;
            }
        }
    }
}