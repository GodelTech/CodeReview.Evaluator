using System;
using StoryLine.Contracts;

namespace CodeReview.Evaluator.IntegrationTests.Actions
{
    public class RunEvaluateAction : IAction
    {
        private readonly string _sqliteDatabaseResourceName;
        private readonly string _manifestResourceName;

        public RunEvaluateAction(string sqliteDatabaseResourceName, string manifestResourceName)
        {
            if (string.IsNullOrWhiteSpace(sqliteDatabaseResourceName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(sqliteDatabaseResourceName));
            if (string.IsNullOrWhiteSpace(manifestResourceName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(manifestResourceName));
            
            _sqliteDatabaseResourceName = sqliteDatabaseResourceName;
            _manifestResourceName = manifestResourceName;
        }

        public void Execute(IActor actor)
        {
            if (actor == null) 
                throw new ArgumentNullException(nameof(actor));

            throw new NotImplementedException();
        }
    }
}