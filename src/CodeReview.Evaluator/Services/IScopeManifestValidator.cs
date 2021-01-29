using ReviewItEasy.Evaluator.Models;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IScopeManifestValidator
    {
        bool IsValid(ScopeManifest manifest);
    }
}