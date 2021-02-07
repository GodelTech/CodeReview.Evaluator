using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IFileService _fileService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IDbRequestExecutorFactory _executorFactory;

        public EvaluationService(
            IFileService fileService, 
            IJsonSerializer jsonSerializer,
            IDbRequestExecutorFactory executorFactory)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _executorFactory = executorFactory ?? throw new ArgumentNullException(nameof(executorFactory));
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
                var queryResult = await executor.ExecuteAsync(requestName);

                if (dbRequestManifest.AddToOutput)
                    result.Add(requestName, queryResult);
            }
            
            var json = _jsonSerializer.Serialize(result);

            await _fileService.WriteAllTextAsync(outputFilePath, json);
        }
    }
}
