using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Advanced.DataContracts;

namespace Prometheus.Client.Internal
{
    public class LabelValues
    {
        public static readonly LabelValues Empty = new LabelValues(new string[0], new string[0]);
        private readonly string[] _values;
        internal readonly List<LabelPair> WireLabels = new List<LabelPair>();


        public LabelValues(IReadOnlyCollection<string> names, string[] values)
        {
            if (names.Count != values.Length)
                throw new InvalidOperationException("Label values must be of same length as label names");
            _values = values;
            WireLabels.AddRange(names.Zip(values, (s, s1) => new LabelPair {name = s, value = s1}));
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            var other = (LabelValues) obj;

            if (other._values.Length != _values.Length) return false;
            for (var i = 0; i < _values.Length; i++)
                if (_values[i] != other._values[i]) return false;

            return true;
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
            throw new NotSupportedException();
        }
    }
}