using System;

namespace Prometheus.Client.Tests;

public static class StringExtensions
{
    private const string _unix = "\n";
    private const string _nonUnix = "\r\n";

    public static string ToUnixLineEndings(this string s)
    {
        return Environment.NewLine == _unix ? s : s.Replace(_nonUnix, _unix);
    }
}
