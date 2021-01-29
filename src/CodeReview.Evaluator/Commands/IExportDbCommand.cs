using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Options;

namespace ReviewItEasy.Evaluator.Commands
{
    public interface IExportDbCommand
    {
        Task<int> ExecuteAsync(ExportDbOptions options);
    }
}