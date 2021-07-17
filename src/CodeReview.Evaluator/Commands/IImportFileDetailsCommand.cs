using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public interface IImportFileDetailsCommand
    {
        Task<int> ExecuteAsync(ImportFileDetailsOptions options);
    }
}