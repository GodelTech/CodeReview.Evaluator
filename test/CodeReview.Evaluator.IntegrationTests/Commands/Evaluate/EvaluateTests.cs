using CodeReview.Evaluator.IntegrationTests.Expectations;
using StoryLine;
using Xunit;

namespace CodeReview.Evaluator.IntegrationTests.Commands.Evaluate
{
    public class EvaluateTests
    {
        [Fact]
        public void When_ScalarResult_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }

        [Fact]
        public void When_ObjectResultIntResult_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }

        [Fact]
        public void When_ScalarResultReferenceToQueryResult_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }

        [Fact]
        public void When_TwoScalarResultsOnlyOneGoesToOutput_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }

        [Fact]
        public void When_QueryResult_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }

        [Fact]
        public void When_ObjectResult_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }

        [Fact]
        public void When_ScalarResultIsMappedToRange_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }

        [Fact]
        public void When_ScalarResultWithParametrizedQuery_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }

        [Fact]
        public void When_CollectionResultWithQueryReference_Should_ReturnExpectedJson()
        {
            Scenario.New()
                .When()
                    .Performs<Actions.Evaluate>(x => x.WithManifestFromResources())
                .Then()
                    .Expects<EvaluationResult>(x => x.WithOutputFromResources())
                .Run();
        }
    }
}
