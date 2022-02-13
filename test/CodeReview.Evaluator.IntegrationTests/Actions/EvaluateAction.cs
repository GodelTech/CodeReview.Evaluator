using System;
using CodeReview.Evaluator.IntegrationTests.Actions.Artifacts;
using CodeReview.Evaluator.IntegrationTests.Utils;
using FluentAssertions;
using StoryLine;
using StoryLine.Contracts;
using StoryLine.Utils.Expectations;

namespace CodeReview.Evaluator.IntegrationTests.Actions
{
    public class EvaluateAction : IAction
    {
        private static readonly TempFileFactory FileFactory = new();
        private static readonly ResourceContentProvider ContentProvider = new();

        private readonly string _sqliteDatabaseResourceName;
        private readonly string _manifestResourceName;

        public EvaluateAction(string sqliteDatabaseResourceName, string manifestResourceName)
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

            using var sqliteDbFile = FileFactory.Create();
            using var manifestFile = FileFactory.Create();
            using var outputFile = FileFactory.Create();

            sqliteDbFile.Write(ContentProvider.ReadAsStream(_sqliteDatabaseResourceName));
            manifestFile.Write(ContentProvider.ReadAsString(_manifestResourceName));

            Scenario.New()
                .When()
                    .Performs<RunEvaluator>(x => x
                        .WithCommand("evaluate")
                        .WithParameter("-d", sqliteDbFile.FilePath)
                        .WithParameter("-m", manifestFile.FilePath)
                        .WithParameter("-o", outputFile.FilePath)
                        )
                .Then()
                    .Expects<Artifact<ExecutionResult>>(x => x
                        .Meets(p => p.ReturnCode.Should().Be(0)))
                .Run();

            var evaluationResult = new EvaluationResult
            {
                FileExists = outputFile.Exists,
                Content = outputFile.ReadAllText()
            };

            actor.Artifacts.Add(evaluationResult);
        }
    }
}