using System.Collections.Generic;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IIssueService
    {
        Task<IEnumerable<Issue>> GetIssuesAsync(IssueProcessingOptionsBase options);
    }
}