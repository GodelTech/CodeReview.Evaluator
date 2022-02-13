using System.IO;

namespace CodeReview.Evaluator.IntegrationTests.Utils
{
    internal class TempFileFactory
    {
        public TempFile Create()
        {
            return new TempFile(Path.GetTempFileName());
        }
    }
}
