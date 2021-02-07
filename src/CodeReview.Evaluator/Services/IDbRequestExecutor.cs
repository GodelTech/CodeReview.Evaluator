using System.Threading.Tasks;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IDbRequestExecutor
    {
        Task<object> ExecuteAsync(string queryName);
    }
}