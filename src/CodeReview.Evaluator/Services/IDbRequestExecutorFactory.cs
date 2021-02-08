using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IDbRequestExecutorFactory
    {
        IDbRequestExecutor Create(EvaluationManifest manifest, string dbFilePath);
    }
}