using System.Collections.Generic;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public interface IIssueProvider
    {
        IEnumerable<Issue> ReadAllIssues(string issuesFilePath);
    }
}