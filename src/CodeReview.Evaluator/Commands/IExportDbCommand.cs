using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public interface IExportDbCommand
    {
        Task<int> ExecuteAsync(ExportDbOptions options);
    }
}