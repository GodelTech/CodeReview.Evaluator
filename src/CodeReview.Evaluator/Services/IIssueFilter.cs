using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IIssueFilter
    {
        bool IsMatch(Issue issue);
    }
}