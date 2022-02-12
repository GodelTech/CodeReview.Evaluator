using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodeReview.Evaluator.IntegrationTests.Utils
{
    internal class ResourceNameResolver
    {
        private static readonly HashSet<string> AssemblyResourceNames = new(typeof(ResourceNameResolver).Assembly.GetManifestResourceNames(), StringComparer.InvariantCultureIgnoreCase);
        private static readonly TestMethodAttributeBasedFilter AttributeBasedFilter = new();

        // Order is important for performance reasons
        private static readonly string[] Suffixes =
        {
            ".approved.",
            ""
        };

        public string FindResourceInResourceFolder(string relativeName)
        {
            var expectedResourceName = "CodeReview.Evaluator.IntegrationTests.Resources" + relativeName;

            return AssemblyResourceNames.TryGetValue(expectedResourceName, out var actualResourceName) ? 
                actualResourceName : 
                string.Empty;
        }

        public string FindResultResourceNameByMethod()
        {
            var trace = new StackTrace();

            var frames = trace.GetFrames();

            var frame = frames.FirstOrDefault(AttributeBasedFilter.IsMatch);
            if (frame == null)
                return string.Empty;

            return GetResourceName(frame);
        }

        private static string GetResourceName(StackFrame frame)
        {
            var typeName = frame.GetType().FullName + "." + frame.GetMethod()?.Name;

            foreach (var suffix in Suffixes)
            {
                var matchingResource = AssemblyResourceNames.FirstOrDefault(x => x.Equals(typeName + suffix, StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrEmpty(matchingResource))
                    return matchingResource;
            }

            return string.Empty;
        }
    }
}