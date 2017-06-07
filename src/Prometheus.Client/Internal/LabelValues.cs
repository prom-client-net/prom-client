using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Contracts;

namespace Prometheus.Client.Internal
{
    public class LabelValues
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

            WireLabels.AddRange(names.Zip(values, (s, s1) => new LabelPair { name = s, value = s1 }));
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            var labelValues = (LabelValues)obj;

            if (labelValues._values.Length != _values.Length)
                return false;

            return !_values.Where((t, i) => t != labelValues._values[i]).Any();
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