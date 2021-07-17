using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public interface IImportIssuesCommand
    {
        Task<int> ExecuteAsync(ImportIssuesOptions options);
    }
}