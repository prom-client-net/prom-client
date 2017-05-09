using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("Prometheus.Client.Tests")]

namespace Prometheus.Client.Internal
{
    internal struct ThreadSafeLong
    {
        private long _value;

        public ThreadSafeLong(long value)
        {
            _value = value;
        }

        public long Value
        {
            get => Interlocked.Read(ref _value);
            set => Interlocked.Exchange(ref _value, value);
        }

        public void Add(long increment)
        {
            while (true)
            {
                long initialValue = _value;
                long computedValue = initialValue + increment;

                if (initialValue == Interlocked.CompareExchange(ref _value, computedValue, initialValue))
                    return;
            }
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            if (obj is ThreadSafeLong)
                return Value.Equals(((ThreadSafeLong)obj).Value);

            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
