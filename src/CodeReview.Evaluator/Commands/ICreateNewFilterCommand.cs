using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public interface ICreateNewFilterCommand
    {
        Task<int> ExecuteAsync(NewFilterOptions options);
    }
}