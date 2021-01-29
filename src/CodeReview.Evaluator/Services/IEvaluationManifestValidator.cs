using ReviewItEasy.Evaluator.Models;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IEvaluationManifestValidator
    {
        bool IsValid(EvaluationManifest manifest);
    }
}