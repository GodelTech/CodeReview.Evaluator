using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IFileLocDetailsProvider
    {
        Task<FileLocDetails[]> GetDetailsAsync(ImportFileDetailsOptions options);
    }
}