using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("Prometheus.Client.Tests")]

namespace Prometheus.Client.Internal
{
    internal struct ThreadSafeDouble : IEquatable<ThreadSafeDouble>
    {
        private long _value;

        public ThreadSafeDouble(double value)
        {
            _value = BitConverter.DoubleToInt64Bits(value);
        }

        public double Value
        {
            get => BitConverter.Int64BitsToDouble(Interlocked.Read(ref _value));
            set => Interlocked.Exchange(ref _value, BitConverter.DoubleToInt64Bits(value));
        }

        public void Add(double increment)
        {
            while (true)
            {
                var initialValue = Interlocked.Read(ref _value);
                var computedValue = BitConverter.Int64BitsToDouble(initialValue) + increment;

                if (initialValue == Interlocked.CompareExchange(ref _value, BitConverter.DoubleToInt64Bits(computedValue), initialValue))
                    return;
            }
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public bool Equals(ThreadSafeDouble threadSafeLong)
        {
            return Value.Equals(threadSafeLong.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is ThreadSafeDouble d)
                return Equals(d);

            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}