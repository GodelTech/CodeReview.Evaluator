using System.Collections.Generic;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IFileListResolver
    {
        IEnumerable<string> ResolveFiles(IssueProcessingOptionsBase options);
    }
}