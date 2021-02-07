using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Services.IssueFilters;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IIssueFilterFactory
    {
        IIssueFilter Create(ScopeManifest manifest);
    }
}