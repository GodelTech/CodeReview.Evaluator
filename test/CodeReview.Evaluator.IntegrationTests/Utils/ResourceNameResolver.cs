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

        public string FindResourceInResourceFolder(string relativeName)
        {
            var expectedResourceName = "CodeReview.Evaluator.IntegrationTests.Resources" + relativeName;

            return AssemblyResourceNames.TryGetValue(expectedResourceName, out var actualResourceName) ?
                actualResourceName :
                string.Empty;
        }

        public string FindResultResourceNameByMethod(string suffix = "")
        {
            var trace = new StackTrace();

            var frames = trace.GetFrames();

            var frame = frames.FirstOrDefault(AttributeBasedFilter.IsMatch);

            return frame == null ? 
                string.Empty : 
                GetResourceName(frame, suffix);
        }

        private static string GetResourceName(StackFrame frame, string suffix)
        {
            var typeName = frame.GetType().FullName + "." + frame.GetMethod()?.Name;

            var matchingResource = AssemblyResourceNames.FirstOrDefault(x => x.Equals(typeName + suffix, StringComparison.InvariantCultureIgnoreCase));

            return !string.IsNullOrEmpty(matchingResource) ? 
                matchingResource : 
                string.Empty;
        }
    }
}