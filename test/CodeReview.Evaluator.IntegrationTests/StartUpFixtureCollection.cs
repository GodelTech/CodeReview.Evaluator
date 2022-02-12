using Xunit;

namespace CodeReview.Evaluator.IntegrationTests
{
    [CollectionDefinition(nameof(StartUpFixture))]
    public class StartUpFixtureCollection : ICollectionFixture<StartUpFixture>
    {
    }
}