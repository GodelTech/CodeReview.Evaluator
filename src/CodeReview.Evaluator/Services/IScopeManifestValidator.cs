using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IScopeManifestValidator
    {
        bool IsValid(ScopeManifest manifest);
    }
}