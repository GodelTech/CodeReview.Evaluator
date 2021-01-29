using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IEvaluationManifestValidator
    {
        bool IsValid(EvaluationManifest manifest);
    }
}