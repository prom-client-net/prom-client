using System;
using System.Threading;

namespace Prometheus.Client;

internal struct ThreadSafeDouble
{
    private long _value;
    private bool _isNan;

    public ThreadSafeDouble(double value)
    {
        _value = BitConverter.DoubleToInt64Bits(value);
        _isNan = IsNaN(value);
    }

    public double Value
    {
        get
        {
            return BitConverter.Int64BitsToDouble(Interlocked.Read(ref _value));
        }
        set
        {
            if (IsNaN(value))
                Volatile.Write(ref _isNan, true);

            Interlocked.Exchange(ref _value, BitConverter.DoubleToInt64Bits(value));
        }
    }

    public void Add(double increment)
    {
        if (IsNaN(increment))
            throw new InvalidOperationException("Cannot increment the NaN value.");

        long currentValue = Interlocked.Read(ref _value);

        while (true)
        {
            if (Volatile.Read(ref _isNan))
                throw new InvalidOperationException("Cannot increment the NaN value.");

            double computedValue = BitConverter.Int64BitsToDouble(currentValue) + increment;

            var lastValue = Interlocked.CompareExchange(ref _value, BitConverter.DoubleToInt64Bits(computedValue), currentValue);

            if (lastValue == currentValue)
                return;

            currentValue = lastValue;
        }
    }

    public void IncTo(double value)
    {
        if (IsNaN(value))
            throw new InvalidOperationException("Cannot increment the NaN value.");

        long currentValue = Interlocked.Read(ref _value);

        while (true)
        {
            if (Volatile.Read(ref _isNan))
                throw new InvalidOperationException("Cannot increment the NaN value.");

            double decodedValue = BitConverter.Int64BitsToDouble(currentValue);

            if (decodedValue >= value)
                return;

            var lastValue = Interlocked.CompareExchange(ref _value, BitConverter.DoubleToInt64Bits(value), currentValue);

            if (lastValue == currentValue)
                return;

            currentValue = lastValue;
        }
    }

    public void DecTo(double value)
    {
        if (IsNaN(value))
            throw new InvalidOperationException("Cannot decrement the NaN value.");

        long currentValue = Interlocked.Read(ref _value);

        while (true)
        {
            if (Volatile.Read(ref _isNan))
                throw new InvalidOperationException("Cannot decrement the NaN value.");

            double decodedValue = BitConverter.Int64BitsToDouble(currentValue);

            if (decodedValue <= value)
                return;

            var lastValue = Interlocked.CompareExchange(ref _value, BitConverter.DoubleToInt64Bits(value), currentValue);

            if (lastValue == currentValue)
                return;

            currentValue = lastValue;
        }
    }

    internal static bool IsNaN(double val)
    {
        if (val >= 0d || val < 0d)
            return false;

        return true;
    }
}
