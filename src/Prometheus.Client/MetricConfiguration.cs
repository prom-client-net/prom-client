using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prometheus.Client.Collectors;

namespace Prometheus.Client;

public class MetricConfiguration : CollectorConfiguration
{
    public MetricConfiguration(string name, string help, string[] labels, bool includeTimestamp, TimeSpan timeToLive)
        : base(name)
    {
        Help = help;
        IncludeTimestamp = includeTimestamp;
        TimeToLive = timeToLive == TimeSpan.MaxValue ? TimeSpan.Zero : timeToLive;
        LabelNames = labels ?? Array.Empty<string>();

        if (labels != null)
        {
            foreach (string labelName in labels)
            {
                if (string.IsNullOrEmpty(labelName))
                    throw new ArgumentException("Label name cannot be empty");

                if (!ValidateLabelName(labelName))
                    throw new ArgumentException($"Invalid label name: {labelName}");
            }
        }
    }

    public string Help { get; }

    public bool IncludeTimestamp { get; }

    public TimeSpan TimeToLive { get; }

    public IReadOnlyList<string> LabelNames { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ValidateLabelName(string labelName)
    {
        if (labelName.Length >= 2 && labelName[0] == '_' && labelName[1] == '_')
            return false;

        if (char.IsDigit(labelName[0]))
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
