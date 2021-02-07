using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Exceptions;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class DbRequestExecutor : IDbRequestExecutor
    {
        private const int MaxRecursionDepth = 100;

        private readonly Dictionary<string, object> _cache = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, DbRequestManifest> _requests;
        
        private readonly EvaluationManifest _manifest;
        private readonly string _dbFilePath;
        private readonly IDatabaseService _databaseService;

        public DbRequestExecutor(
            EvaluationManifest manifest, 
            string dbFilePath,
            IDatabaseService databaseService)
        {
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));
            
            _manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            
            _dbFilePath = dbFilePath;

            _requests = manifest.Requests.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
        }

        public Task<object> ExecuteAsync(string queryName)
        {
            if (string.IsNullOrWhiteSpace(queryName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(queryName));
            
            return ExecuteRecursiveAsync(0, queryName);
        }

        private async Task<object> ExecuteRecursiveAsync(int recursionDepth, string queryName)
        {
            if (recursionDepth >= MaxRecursionDepth)
                throw new EvaluateException("Max recursion depth reached");
            
            if (_cache.TryGetValue(queryName, out var result))
                return result;

            var dbRequestManifest = _requests[queryName];

            var executionResult = await ExecuteRequestAsync(recursionDepth, dbRequestManifest);

            _cache.Add(queryName, executionResult);
            
            return executionResult;
        }

        private async Task<object> ExecuteRequestAsync(int recursionDepth, DbRequestManifest dbRequestManifest)
        {
            var parameters = await ResolveParameterAsync(recursionDepth, dbRequestManifest.Parameters);
            var queryText = GetQueryText(dbRequestManifest);

            switch (dbRequestManifest.Type)
            {
                case RequestType.Scalar:
                    return await _databaseService.ExecuteScalarAsync(_dbFilePath, queryText, parameters);

                case RequestType.Object:
                    return await _databaseService.ExecuteObjectAsync(_dbFilePath, queryText, parameters);

                case RequestType.Collection:
                    return await _databaseService.ExecuteCollectionAsync(_dbFilePath, queryText, parameters);

                case RequestType.NoResult:
                    await _databaseService.ExecuteNonQueryAsync(_dbFilePath, queryText, parameters);
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbRequestManifest.Type));
            }
        }

        private async Task<Dictionary<string, object>> ResolveParameterAsync(int recursionDepth, Dictionary<string, ParameterManifest> parameterManifests)
        {
            var parameters = new Dictionary<string, object>();
            
            foreach (var (paramName, paramManifest) in parameterManifests)
            {
                var value = await ResolveParameterValue(recursionDepth, paramManifest);

                parameters.Add(paramName, value);
            }

            return parameters;
        }

        private string GetQueryText(DbRequestManifest dbRequestManifest)
        {
            if (!string.IsNullOrWhiteSpace(dbRequestManifest.Query))
                return dbRequestManifest.Query;

            if (string.IsNullOrWhiteSpace(dbRequestManifest.QueryRef))
                return string.Empty;

            if (_manifest.Queries.TryGetValue(dbRequestManifest.QueryRef, out var manifest))
                return manifest.Query;

            throw new QueryExecutionException("Failed to resolve query text");
        }

        private async Task<object> ResolveParameterValue(int recursionDepth, ParameterManifest parameterManifest)
        {
            if (parameterManifest.IsNull)
                return DBNull.Value;

            if (parameterManifest.IsInt)
                return long.Parse(parameterManifest.Value);

            if (parameterManifest.IsValueRef)
                return await ExecuteRecursiveAsync(recursionDepth + 1, parameterManifest.Value);

            return parameterManifest.Value;
        }
    }
}