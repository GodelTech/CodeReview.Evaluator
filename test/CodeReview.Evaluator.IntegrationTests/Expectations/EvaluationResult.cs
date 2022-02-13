using CodeReview.Evaluator.IntegrationTests.Utils;
using StoryLine.Contracts;

namespace CodeReview.Evaluator.IntegrationTests.Expectations
{
    internal class EvaluationResult : IExpectationBuilder
    {
        private static readonly ResourceNameResolver ResourceNameResolver = new();
        private string _expectedResultResourceName;

        public EvaluationResult WithOutputFromResources(string suffix = ".output.")
        {
            _expectedResultResourceName = ResourceNameResolver.FindResultResourceNameByMethod(suffix);

            return this;
        }

        public IExpectation Build()
        {
            return new EvaluationResultExpectation(_expectedResultResourceName);
        }
    }
}
