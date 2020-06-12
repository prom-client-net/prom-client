using System;

namespace Prometheus.Client.SummaryImpl
{
    // Sample holds an observed value and meta information for compression. 
    internal readonly struct Sample
    {
        public static Sample Empty = default;

        public Sample(double value, int width, int delta)
        {
            Value = value;
            Width = width;
            Delta = delta;
        }

        public static bool IsEmpty(Sample sample)
        {
            return Math.Abs(sample.Value - 0) < double.Epsilon
                   && sample.Width == default
                   && sample.Delta == default;
        }

        public double Value { get; }
        public int Width { get; }
        public int Delta { get; }
    }
}
