using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IScopeManifestProvider
    {
        Task<ScopeManifest> GetScopeManifestAsync(string scopeManifestPath);
    }
}