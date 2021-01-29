using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Options;

namespace ReviewItEasy.Evaluator.Commands
{
    public interface ICreateNewFilterCommand
    {
        Task<int> ExecuteAsync(NewFilterOptions options);
    }
}