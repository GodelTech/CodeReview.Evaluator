using CodeReview.Evaluator.IntegrationTests.Utils;
using StoryLine.Contracts;

namespace CodeReview.Evaluator.IntegrationTests.Actions
{
    public class RunEvaluate : IActionBuilder
    {
        private static readonly ResourceNameResolver ResourceNameResolver = new();
        private static readonly string SqliteDatabaseResourceName = ResourceNameResolver.FindResourceInResourceFolder("issues.db");

        private string _manifestResourceName;

        public IActionBuilder WithManifestFromResources()
        {
            _manifestResourceName = ResourceNameResolver.FindResultResourceNameByMethod();

            return this;
        }

        public IAction Build()
        {
            return new RunEvaluateAction(
                SqliteDatabaseResourceName,
                _manifestResourceName);
        }
    }
}
