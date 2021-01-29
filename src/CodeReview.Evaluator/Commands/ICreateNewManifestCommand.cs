using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Options;

namespace ReviewItEasy.Evaluator.Commands
{
    public interface ICreateNewManifestCommand
    {
        Task<int> ExecuteAsync(NewManifestOptions options);
    }
}