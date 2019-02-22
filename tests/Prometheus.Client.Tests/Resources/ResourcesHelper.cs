using System.IO;
using System.Reflection;

namespace Prometheus.Client.Tests.Resources
{
    internal static class ResourcesHelper
    {
        public static string GetFileContent(string fileName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Prometheus.Client.Tests.Resources.{fileName}");
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
