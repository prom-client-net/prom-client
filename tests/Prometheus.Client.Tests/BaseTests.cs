using System.Collections.Generic;

namespace Prometheus.Client.Tests
{
    public class BaseTests
    {
        public static IEnumerable<object[]> GetLabels()
        {
            yield return new object[] { null };
            yield return new object[] { null, null };
            yield return new object[] { "onlyone", null };
            yield return new object[] { "one", "two", "three" };
        }

        public static IEnumerable<object[]> InvalidLabels()
        {
            yield return new object[] { "my-metric" };
            yield return new object[] { "my!metric" };
            yield return new object[] { "my%metric" };
        }
    }
}
