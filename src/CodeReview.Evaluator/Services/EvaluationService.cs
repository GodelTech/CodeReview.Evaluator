using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Exceptions;
using ReviewItEasy.Evaluator.Models;

namespace ReviewItEasy.Evaluator.Services
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

            var result = new EvaluationResult();
            
            foreach (var (scalarName, dbRequestManifest) in manifest.Scalars)
            {
                var queryResult = await _databaseService.ExecuteScalarAsync(dbFilePath, GetQueryText(dbRequestManifest, manifest.Queries), dbRequestManifest.Parameters);
               
                result.Scalars.Add(scalarName, queryResult);
            }

            foreach (var (objectName, dbRequestManifest) in manifest.Objects)
            {
                var queryResult = await _databaseService.ExecuteObjectAsync(dbFilePath, GetQueryText(dbRequestManifest, manifest.Queries), dbRequestManifest.Parameters);

                result.Objects.Add(objectName, queryResult);
            }

            foreach (var (collectionName, dbRequestManifest) in manifest.Collections)
            {
                var queryResult = await _databaseService.ExecuteCollectionAsync(dbFilePath, GetQueryText(dbRequestManifest, manifest.Queries), dbRequestManifest.Parameters);

                result.Collections.Add(collectionName, queryResult);
            }

            var json = _jsonSerializer.Serialize(result);

            await _fileService.WriteAllTextAsync(outputFilePath, json);
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
