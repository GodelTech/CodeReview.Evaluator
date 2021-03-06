﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IFileService _fileService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IDbRequestExecutorFactory _executorFactory;
        private readonly ILogger<EvaluationService> _logger;

        public EvaluationService(
            IFileService fileService, 
            IJsonSerializer jsonSerializer,
            IDbRequestExecutorFactory executorFactory,
            ILogger<EvaluationService> logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _executorFactory = executorFactory ?? throw new ArgumentNullException(nameof(executorFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task EvaluateAsync(EvaluationManifest manifest, string dbFilePath, string outputFilePath)
        {
            if (manifest == null) 
                throw new ArgumentNullException(nameof(manifest));
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));
            if (string.IsNullOrWhiteSpace(outputFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(outputFilePath));

            var executor = _executorFactory.Create(manifest, dbFilePath);
            var result = new Dictionary<string, object>();
            
            foreach (var (requestName, dbRequestManifest) in manifest.Requests)
            {
                _logger.LogInformation("Starting evaluation of \"{request}\" request...",  requestName);
                
                var queryResult = await executor.ExecuteAsync(requestName);

                if (dbRequestManifest.AddToOutput)
                    result.Add(requestName, ResolveResult(queryResult, dbRequestManifest));

                _logger.LogInformation("Evaluation completed", requestName);
            }

            var json = _jsonSerializer.Serialize(result);

            await _fileService.WriteAllTextAsync(outputFilePath, json);
        }

        private static object ResolveResult(object queryResult, DbRequestManifest dbRequestManifest)
        {
            if (dbRequestManifest.Type != RequestType.Scalar)
                return queryResult;

            if (!dbRequestManifest.Ranges.Any())
                return queryResult;

            return new
            {
                Value = queryResult,
                Range = ResolveRange(queryResult, dbRequestManifest.Ranges)
            };
        }

        private static string ResolveRange(object queryResult, Dictionary<string, ValueRange> ranges)
        {
           return
               (from item in ranges
               where item.Value.IsInRange(queryResult)
               select item.Key)
               .FirstOrDefault();
        }
    }
}
