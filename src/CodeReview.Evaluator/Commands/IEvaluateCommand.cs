using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public interface IEvaluateCommand
    {
        Task<int> ExecuteAsync(EvaluateOptions options);
    }
}