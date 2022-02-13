using System;
using CodeReview.Evaluator.IntegrationTests.Utils;
using FluentAssertions;
using StoryLine.Contracts;

namespace CodeReview.Evaluator.IntegrationTests.Expectations
{
    internal class EvaluationResultExpectation : IExpectation
    {
        private static readonly JsonFormatter JsonFormatter = new();
        private static readonly ResourceContentProvider ContentProvider = new();

        private readonly string _expectedResultResourceName;

        public EvaluationResultExpectation(string expectedResultResourceName)
        {
            if (string.IsNullOrWhiteSpace(expectedResultResourceName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(expectedResultResourceName));
            _expectedResultResourceName = expectedResultResourceName;
        }

        public void Validate(IActor actor)
        {
            if (actor == null) 
                throw new ArgumentNullException(nameof(actor));

            var result = actor.Artifacts.Get<Actions.Artifacts.EvaluationResult>();

            result.Should().NotBeNull();
            result.FileExists.Should().BeTrue();

            var expectedResult = ContentProvider.ReadAsString(_expectedResultResourceName);

            var expectedFormattedResult = JsonFormatter.Format(expectedResult);
            var actualFormattedResult = JsonFormatter.Format(result.Content);

            actualFormattedResult.Should().Be(expectedFormattedResult);
        }
    }
}