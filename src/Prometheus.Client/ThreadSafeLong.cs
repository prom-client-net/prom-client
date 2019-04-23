using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("Prometheus.Client.Tests" +
    ", PublicKey=00240000048000009400000006020000002400005253413100040000010001006155579b902d58e0a83000c846d41d9f9b98ab0f03c38f7d77b9221617a834d188db1d5b310b8449504d96647bf9b90f9446f46f133f7bbf649e4e3bff0c4031c16571847789bf9074526fac893ae8370020705b8b0e88212f2828806fb39029959202aa2add7f0fd33162b8e846184990ba26054c1aa3d8241ecca6bb6e6fca")]

namespace Prometheus.Client
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
