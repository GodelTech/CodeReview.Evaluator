using FluentAssertions;
using GodelTech.CodeReview.Evaluator.Models;
using Xunit;

namespace CodeReview.Evaluator.Tests.Models
{
    public class ValueRangeTests
    {
        [Theory]
        [InlineData(1, 3, 2, true)]
        [InlineData(1, 3, 3, false)]
        [InlineData(1, 3, 1, true)]
        [InlineData(null, 1, -1, true)]
        [InlineData(1, null, 1, true)]
        [InlineData(null, 1, 1, false)]
        [InlineData(1, 3, null, false)]
        [InlineData(1, 3, "xxxx", false)]
        [InlineData(1, 3, "2", true)]
        public void IsInRange_Should_Return_ExpectedResult(int? start, int? end, object value, bool expectedResult)
        {
            var range = new ValueRange
            {
                Start = start.HasValue ? (decimal)start.Value : null,
                End = end.HasValue ? (decimal)end.Value : null
            };

            range.IsInRange(value).Should().Be(expectedResult);
        }
    }
}
