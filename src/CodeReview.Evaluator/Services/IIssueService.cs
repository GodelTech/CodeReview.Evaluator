using System.Collections.Generic;
using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Models;
using ReviewItEasy.Evaluator.Options;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IIssueService
    {
        Task<IEnumerable<Issue>> GetIssuesAsync(IssueProcessingOptionsBase options);
    }
}