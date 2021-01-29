using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public interface ICreateNewManifestCommand
    {
        Task<int> ExecuteAsync(NewManifestOptions options);
    }
}