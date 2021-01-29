using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Models;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IEvaluationService
    {
        Task EvaluateAsync(EvaluationManifest manifest, string dbFilePath, string outputFilePath);
    }
}