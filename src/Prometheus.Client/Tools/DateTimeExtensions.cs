using System;

namespace Prometheus.Client.Tools
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTime(this DateTime date)
        {
            return (long)(date - _epoch).TotalMilliseconds;
        }

        public static long ToUnixTimeSeconds(this DateTime date)
        {
            return (long)(date - _epoch).TotalSeconds;
        }

        public static long ToUnixTime(this DateTimeOffset date)
        {
            return (long)(date - _epoch).TotalMilliseconds;
        }
    }
}
