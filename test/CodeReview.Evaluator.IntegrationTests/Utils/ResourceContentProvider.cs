using System;
using System.IO;
using System.Reflection;

namespace CodeReview.Evaluator.IntegrationTests.Utils
{
    public class ResourceContentProvider
    {
        private static readonly Assembly ResourceAssembly = typeof(ResourceContentProvider).Assembly;

        public string ReadAsString(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(resourceName));

            var stream = ReadAsStream(resourceName);
            if (stream == null)
                return string.Empty;

            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public Stream ReadAsStream(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(resourceName));

            return ResourceAssembly.GetManifestResourceStream(resourceName);
        }
    }
}
