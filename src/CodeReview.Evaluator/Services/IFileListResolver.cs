using System.Collections.Generic;
using ReviewItEasy.Evaluator.Options;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IFileListResolver
    {
        IEnumerable<string> ResolveFiles(IssueProcessingOptionsBase options);
    }
}