using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services.IssueFilters
{
    public interface IIssueFilter
    {
        bool IsMatch(Issue issue);
    }
}