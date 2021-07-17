using System.Collections.Generic;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IDatabaseService
    {
        Task<bool> DoesTableExist(string dbFilePath, string tableName);

        Task CreateFileDetailsDbAsync(string dbFilePath);
        Task CreateIssuesDbAsync(string dbFilePath);
        Task SaveIssuesAsync(string dbFilePath, IEnumerable<Issue> issues);
        Task SaveLocDetailsAsync(string dbFilePath, FileLocDetails[] items);

        Task ExecuteNonQueryAsync(string dbFilePath, string queryText, IReadOnlyDictionary<string, object> parameters);
        Task<object> ExecuteScalarAsync(string dbFilePath, string queryText, IReadOnlyDictionary<string, object> parameters);
        Task<object> ExecuteObjectAsync(string dbFilePath, string queryText, IReadOnlyDictionary<string, object> parameters);
        Task<object> ExecuteCollectionAsync(string dbFilePath, string queryText, IReadOnlyDictionary<string, object> parameters);
    }
}