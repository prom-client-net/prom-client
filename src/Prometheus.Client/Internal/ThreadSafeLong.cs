using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("Prometheus.Client.Tests")]

namespace Prometheus.Client.Internal
{
    internal struct ThreadSafeLong : IEquatable<ThreadSafeLong>
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

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public bool Equals(ThreadSafeLong threadSafeLong)
        {
            return Value.Equals(threadSafeLong.Value);
        }
        
        public override bool Equals(object obj)
        {
            if (obj is ThreadSafeLong l)
                return Equals(l);

            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
