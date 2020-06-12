using System;
using System.Collections.Generic;

namespace Prometheus.Client.SummaryImpl
{
    internal class SampleStream
    {
        private static readonly Predicate<Sample> _isEmptySample = Sample.IsEmpty;

        private readonly Invariant _invariant;
        private readonly List<Sample> _samples = new List<Sample>();

        private int _n;

        public SampleStream(Invariant invariant)
        {
            _invariant = invariant;
        }

        public int Count => _n;

        public void InsertRange(ReadOnlySpan<double> samples)
        {
            // TODO(beorn7): This tries to merge not only individual samples, but
            // whole summaries. The paper doesn't mention merging summaries at
            // all. Unittests show that the merging is inaccurate. Find out how to
            // do merges properly.

            double r = 0;
            int i = 0;

            foreach (var sample in samples)
            {
                bool inserted = false;
                for (; i < _samples.Count; i++)
                {
                    var c = _samples[i];

                    if (c.Value > sample)
                    {
                        // Insert at position i
                        _samples.Insert(i,
                            new Sample(sample, 1, (int)Math.Max(0, Math.Floor(_invariant(this, r)) - 1)));

                        i++;
                        inserted = true;
                        break;
                    }

                    r += c.Width;
                }

                if (!inserted)
                {
                    _samples.Add(new Sample(sample, 1, 0));
                    i++;
                }

                _n++;
                r += 1;
            }

            Compress();
        }

        private void Compress()
        {
            if (_samples.Count < 2)
                return;

            var x = _samples[_samples.Count - 1];
            int xi = _samples.Count - 1;
            double r = _n - 1 - x.Width;

            for (int i = _samples.Count - 2; i >= 0; i--)
            {
                var c = _samples[i];

                if (c.Width + x.Width + x.Delta <= _invariant(this, r))
                {
                    x = new Sample(x.Value, x.Width + c.Width, x.Delta);
                    _samples[xi] = x;
                    _samples[i] = Sample.Empty;
                }
                else
                {
                    x = c;
                    xi = i;
                }

                r -= c.Width;
            }

            _samples.RemoveAll(_isEmptySample);
        }

        public void Reset()
        {
            _samples.Clear();
            _n = 0;
        }

        public double Query(double q)
        {
            if (_samples.Count == 0)
                return double.NaN;

            double t = Math.Ceiling(q * _n);
            t += Math.Ceiling(_invariant(this, t) / 2);
            var p = _samples[0];
            double r = 0;

            for (int i = 1; i < _samples.Count; i++)
            {
                var c = _samples[i];
                r += p.Width;

                if (r + c.Width + c.Delta > t)
                    return p.Value;

                p = c;
            }

            return p.Value;
        }
    }
}
