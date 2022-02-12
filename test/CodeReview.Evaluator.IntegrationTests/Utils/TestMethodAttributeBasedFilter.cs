using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CodeReview.Evaluator.IntegrationTests.Utils
{
    internal class TestMethodAttributeBasedFilter
    {

        private static readonly string[] AttributeNames =
        {
            "TestAttribute",
            "TestCaseAttribute",
            "FactAttribute",
            "TheoryAttribute"
        };

        public bool IsMatch(StackFrame details)
        {
            var type = details.GetType();

            var method = details.GetMethod();
            if (method == null)
                return false;

            var attributes = method.GetCustomAttributes();

            return attributes.Any(x => AttributeNames.Any(p => p == x.GetType().Name));
        }
    }
}