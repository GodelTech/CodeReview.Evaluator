using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface ILocDetailsFilterFactory
    {
        ILocDetailsFilter Create(ScopeManifest manifest);
    }
}