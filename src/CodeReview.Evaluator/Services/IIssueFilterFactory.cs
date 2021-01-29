using ReviewItEasy.Evaluator.Models;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IIssueFilterFactory
    {
        IIssueFilter Create(ScopeManifest manifest);
    }
}