using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Prometheus.Client.Contracts;
using Prometheus.Client.Internal;

namespace Prometheus.Client
{
    public static class ScrapeHandler
    {
    
        public static void ProcessScrapeRequest(
            IEnumerable<CMetricFamily> collected,
            Stream outputStream)
        {
            TextFormatter.Format(outputStream, collected);
        }


      
    }
}