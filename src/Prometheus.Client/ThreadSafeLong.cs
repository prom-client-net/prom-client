using System.Threading;

namespace Prometheus.Client
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
            Interlocked.Add(ref _value, increment);
        }

        public void IncTo(long value)
        {
            var currentValue = Interlocked.Read(ref _value);

            while (true)
            {
                if (currentValue >= value)
                    return;

                var lastValue = Interlocked.CompareExchange(ref _value, value, currentValue);
                if (lastValue == currentValue)
                    return;

                currentValue = lastValue;
            }
        }

        public void DecTo(long value)
        {
            var currentValue = Interlocked.Read(ref _value);

            while (true)
            {
                if (currentValue <= value)
                    return;

                var lastValue = Interlocked.CompareExchange(ref _value, value, currentValue);
                if (lastValue == currentValue)
                    return;

                currentValue = lastValue;
            }
        }
    }
}
