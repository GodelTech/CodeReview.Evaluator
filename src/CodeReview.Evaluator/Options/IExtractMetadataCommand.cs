using System.Threading.Tasks;

namespace GodelTech.CodeReview.Evaluator.Options
{
    public interface IExtractMetadataCommand
    {
        Task<int> ExecuteAsync(ExtractMetadataOptions options);
    }
}