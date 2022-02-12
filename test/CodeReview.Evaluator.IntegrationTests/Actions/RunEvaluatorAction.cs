using System;
using GodelTech.CodeReview.Evaluator;
using StoryLine.Contracts;

namespace CodeReview.Evaluator.IntegrationTests.Actions
{
    public class RunEvaluatorAction : IAction
    {
        private readonly string[] _args;

        public RunEvaluatorAction(string[] args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public void Execute(IActor actor)
        {
            if (actor == null) 
                throw new ArgumentNullException(nameof(actor));

            var resultArtifact = new ExecutionResult
            {
                ReturnCode = ExecuteEvaluate()
            };

            actor.Artifacts.Add(resultArtifact);
        }

        private int ExecuteEvaluate()
        {
            try
            {
                return Program.Main(_args);
            }
            catch (Exception)
            {
                return int.MinValue;
            }
        }
    }
}