using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Prometheus.Client.Collectors;

namespace Prometheus.Client
{
    public class MetricConfiguration: CollectorConfiguration
    {
        public MetricConfiguration(string name, string help, IReadOnlyList<string> labels, MetricFlags options)
            : base(name)
        {
            Help = help;
            IncludeTimestamp = options.HasFlag(MetricFlags.IncludeTimestamp);
            SuppressEmptySamples = options.HasFlag(MetricFlags.SupressEmptySamples);
            LabelNames = labels ?? Array.Empty<string>();

            foreach (string labelName in LabelNames)
            {
                if (string.IsNullOrEmpty(labelName))
                    throw new ArgumentException("Label name cannot be empty");

                if (!ValidateLabelName(labelName))
                    throw new ArgumentException($"Invalid label name: {labelName}");
            }
        }

        public string Help { get; }

        public bool IncludeTimestamp { get; }

        public bool SuppressEmptySamples { get; }

        public IReadOnlyList<string> LabelNames { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ValidateLabelName(string labelName)
        {
            if (labelName.Length >=2 && labelName[0] == '_' && labelName[1] == '_')
                return false;

            if (char.IsNumber(labelName[0]))
                return false;

            foreach (var ch in labelName)
            {
                if ((ch >= 'a' && ch <= 'z')
                    || (ch >= 'A' && ch <= 'Z')
                    || char.IsDigit(ch)
                    || ch == '_'
                    || ch == ':')
                    continue;

                return false;
            }

            return true;
        }
    }
}
