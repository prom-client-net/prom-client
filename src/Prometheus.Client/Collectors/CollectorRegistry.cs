using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.Collectors
{
    public class CollectorRegistry : ICollectorRegistry
    {
        public static readonly CollectorRegistry Instance = new CollectorRegistry();
        private readonly ConcurrentDictionary<string, ICollector> _collectors = new ConcurrentDictionary<string, ICollector>();

        public IEnumerable<ICollector> Enumerate() => _collectors.Values;

        public void Clear()
        {
            _collectors.Clear();
        }

        public ICollector Add(ICollector collector)
        {
            if (!_collectors.TryAdd(collector.Name, collector))
            {
                throw new InvalidOperationException($"Collector with name '{collector.Name}' is already registered");
            }

            return collector;
        }

        public ICollector GetOrAdd(ICollector collector)
        {
            var collectorToUse = _collectors.GetOrAdd(collector.Name, collector);

            if (!collector.LabelNames.SequenceEqual(collectorToUse.LabelNames))
                throw new ArgumentException("Collector with same name must have same label names");

            return collectorToUse;
        }

        public bool Remove(ICollector collector)
        {
            return _collectors.TryRemove(collector.Name, out _);
        }
    }
}
