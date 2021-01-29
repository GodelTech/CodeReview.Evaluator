using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IIssueFilterFactory
    {
        IIssueFilter Create(ScopeManifest manifest);
    }
}