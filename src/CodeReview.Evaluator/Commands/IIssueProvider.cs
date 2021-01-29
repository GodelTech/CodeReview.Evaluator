using System.Collections.Generic;
using ReviewItEasy.Evaluator.Models;
using ReviewItEasy.Evaluator.Options;

namespace ReviewItEasy.Evaluator.Commands
{
    public interface IIssueProvider
    {
        IEnumerable<Issue> ReadAllIssues(IssueProcessingOptionsBase options);
    }
}