using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prometheus.Client.Collectors
{
    public readonly struct LabelValues : IEquatable<LabelValues>
    {
        private readonly int _hashCode;
        private readonly string[] _values;

        internal static readonly LabelValues Empty = new LabelValues(new string[0], new string[0]);
        public readonly KeyValuePair<string, string>[] Labels;

        public LabelValues(string[] names, string[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), "Labels cannot be null");

            if (names.Length != values.Length)
                throw new ArgumentException("Incorrect number of labels");

            if (values.Any(value => value == null))
                throw new ArgumentNullException(nameof(values), "Label cannot be null");

            _values = values;

            Labels = names.Zip(values, (name, value) => new KeyValuePair<string, string>(name, value)).ToArray();

            // Calculating the hash code is fast but we don't need to re-calculate it for each comparison this object is involved in.
            // Label values are fixed- caluclate it once up-front and remember the value.
            _hashCode = CalculateHashCode(_values);
        }

        public bool IsEmpty => Labels.Length == 0;

        public bool Equals(LabelValues other)
        {
            if (other._values.Length != _values.Length)
                return false;

            return !_values.Where((t, i) => t != other._values[i]).Any();
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
            foreach (var label in Labels)
                sb.AppendFormat("{0}={1}, ", label.Key, label.Value);

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
                return values.Aggregate(1, (current, t) => current ^ (t.GetHashCode() * 397));
            }
        }
    }
}
