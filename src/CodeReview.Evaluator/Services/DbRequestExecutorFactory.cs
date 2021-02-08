using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class DbRequestExecutorFactory : IDbRequestExecutorFactory
    {
        private readonly IDatabaseService _databaseService;

        public DbRequestExecutorFactory(IDatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }
        
        public IDbRequestExecutor Create(EvaluationManifest manifest, string dbFilePath)
        {
            if (manifest == null) 
                throw new ArgumentNullException(nameof(manifest));
            
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));

            return new DbRequestExecutor(manifest, dbFilePath, _databaseService);
        }
    }
}