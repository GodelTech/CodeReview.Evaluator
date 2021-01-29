using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Options;

namespace ReviewItEasy.Evaluator.Commands
{
    public interface IEvaluateCommand
    {
        Task<int> ExecuteAsync(EvaluateOptions options);
    }
}