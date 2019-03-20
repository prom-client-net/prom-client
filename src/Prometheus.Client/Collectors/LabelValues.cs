using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prometheus.Client.Collectors
{
    public readonly struct LabelValues : IEquatable<LabelValues>
    {
        private readonly int _hashCode;
        private readonly IReadOnlyList<string> _values;

        internal static readonly LabelValues Empty = new LabelValues(Array.Empty<string>(), Array.Empty<string>());
        public readonly IReadOnlyList<KeyValuePair<string, string>> Labels;

        public LabelValues(IReadOnlyList<string> names, IReadOnlyList<string> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), "Labels cannot be null");

            if (names.Count != values.Count)
                throw new ArgumentException("Incorrect number of labels");

            if (values.Any(value => value == null))
                throw new ArgumentNullException(nameof(values), "Label cannot be null");

            _values = values;

            Labels = names.Zip(values, (name, value) => new KeyValuePair<string, string>(name, value)).ToArray();

            // Calculating the hash code is fast but we don't need to re-calculate it for each comparison this object is involved in.
            // Label values are fixed- caluclate it once up-front and remember the value.
            _hashCode = CalculateHashCode(_values);
        }

        public bool IsEmpty => Labels.Count == 0;

        public bool Equals(LabelValues other)
        {
            if (other._values.Count != _values.Count)
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

        private static int CalculateHashCode(IReadOnlyList<string> values)
        {
            unchecked
            {
                return values.Aggregate(1, (current, t) => current ^ (t.GetHashCode() * 397));
            }
        }
    }
}
