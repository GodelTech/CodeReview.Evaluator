using System.Collections.Generic;
using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Models;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IDatabaseService
    {
        Task CreateDbAsync(string dbFilePath);
        Task SaveIssuesAsync(string dbFilePath, IEnumerable<Issue> issues);
        Task ExecuteNonQueryAsync(string dbFilePath, string sql);
        
        Task<object> ExecuteScalarAsync(string dbFilePath, string queryText, Dictionary<string, ParameterManifest> parameters);
        Task<object> ExecuteObjectAsync(string dbFilePath, string queryText, Dictionary<string, ParameterManifest> parameters);
        Task<object> ExecuteCollectionAsync(string dbFilePath, string queryText, Dictionary<string, ParameterManifest> parameters);
    }
}