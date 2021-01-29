using System.Collections.Generic;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public interface IIssueProvider
    {
        IEnumerable<Issue> ReadAllIssues(IssueProcessingOptionsBase options);
    }
}