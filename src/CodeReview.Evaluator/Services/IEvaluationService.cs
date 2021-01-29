using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IEvaluationService
    {
        Task EvaluateAsync(EvaluationManifest manifest, string dbFilePath, string outputFilePath);
    }
}