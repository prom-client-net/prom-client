using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prometheus.Contracts;
using ProtoBuf;

namespace Prometheus.Client.Internal
{
    internal class ProtoFormatter
    {
        public static void Format(Stream destination, IEnumerable<MetricFamily> metrics)
        {
            var metricFamilys = metrics.ToArray();
            foreach (var metricFamily in metricFamilys)
            {
                Serializer.SerializeWithLengthPrefix(destination, metricFamily, PrefixStyle.Base128, 0);
            }
        }
    }
}