using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Services.LocDetailsFilters;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface ILocDetailsFilterFactory
    {
        ILocDetailsFilter Create(ScopeManifest manifest);
    }
}