using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Exceptions;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IFileService _fileService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IDatabaseService _databaseService;

        public EvaluationService(
            IFileService fileService, 
            IJsonSerializer jsonSerializer,
            IDatabaseService databaseService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }
        
        public async Task EvaluateAsync(EvaluationManifest manifest, string dbFilePath, string outputFilePath)
        {
            if (manifest == null) 
                throw new ArgumentNullException(nameof(manifest));
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));
            if (string.IsNullOrWhiteSpace(outputFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(outputFilePath));

            var result = new Dictionary<string, object>();
            
            foreach (var (requestName, dbRequestManifest) in manifest.Requests)
            {
                var queryResult = await ExecuteRequestAsync(manifest, dbFilePath, dbRequestManifest);

                if (queryResult != null)
                    result.Add(requestName, queryResult);
            }
            
            var json = _jsonSerializer.Serialize(result);

            await _fileService.WriteAllTextAsync(outputFilePath, json);
        }

        private async Task<object> ExecuteRequestAsync(EvaluationManifest manifest, string dbFilePath,
            DbRequestManifest dbRequestManifest)
        {
            switch (dbRequestManifest.RequestType)
            {
                case RequestType.Scalar:
                    return await _databaseService.ExecuteScalarAsync(dbFilePath,
                        GetQueryText(dbRequestManifest, manifest.Queries), dbRequestManifest.Parameters);
                
                case RequestType.Object:
                    return await _databaseService.ExecuteObjectAsync(dbFilePath,
                        GetQueryText(dbRequestManifest, manifest.Queries), dbRequestManifest.Parameters);

                case RequestType.Collection:
                    return await _databaseService.ExecuteCollectionAsync(dbFilePath,
                        GetQueryText(dbRequestManifest, manifest.Queries), dbRequestManifest.Parameters);

                case RequestType.NoResult:
                    await _databaseService.ExecuteNonQueryAsync(
                        dbFilePath, GetQueryText(dbRequestManifest, manifest.Queries), dbRequestManifest.Parameters);
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbRequestManifest.RequestType));
            }
        }

        private static string GetQueryText(DbRequestManifest dbRequestManifest, IReadOnlyDictionary<string, QueryManifest> queries)
        {
            if (!string.IsNullOrWhiteSpace(dbRequestManifest.Query))
                return dbRequestManifest.Query;

            if (string.IsNullOrWhiteSpace(dbRequestManifest.QueryRef))
                return string.Empty;

            if (queries.TryGetValue(dbRequestManifest.QueryRef, out var manifest))
                return manifest.Query;

            throw new QueryExecutionException("Failed to resolve query text");
        }
    }
}
