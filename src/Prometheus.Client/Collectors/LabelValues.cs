using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prometheus.Client.Contracts;

namespace Prometheus.Client.Collectors
{
    public struct LabelValues : IEquatable<LabelValues>
    {
        private readonly int _hashCode;
        private readonly string[] _values;

        internal static readonly LabelValues Empty = new LabelValues(new string[0], new string[0]);
        internal readonly List<CLabelPair> WireLabels;

        public LabelValues(string[] names, string[] values)
        {
            if (values == null)
                throw new ArgumentException("Values cannot be null");

            if (names.Length != values.Length)
                throw new ArgumentException("Incorrect number of labels");

            if (values.Any(value => value == null))
                throw new ArgumentException("Label cannot be null");
            
            _values = values;

            WireLabels = new List<CLabelPair>(names.Zip(values, (s, s1) => new CLabelPair { Name = s, Value = s1 }));

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
                sb.AppendFormat("{0}={1}, ", label.Name, label.Value);
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
                return values.Aggregate(1, (current, t) => current ^ t.GetHashCode() * 397);
            }
        }
    }
}