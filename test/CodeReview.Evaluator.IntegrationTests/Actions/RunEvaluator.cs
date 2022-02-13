using System;
using System.Collections.Generic;
using StoryLine.Contracts;

namespace CodeReview.Evaluator.IntegrationTests.Actions
{
    public class RunEvaluator : IActionBuilder
    {
        private string _commandName;
        private readonly Dictionary<string, string> _parameterToValueMap = new();

        public RunEvaluator WithCommand(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(commandName));

            _commandName = commandName;

            return this;
        }

        public RunEvaluator WithParameter(string paramName, string value)
        {
            if (string.IsNullOrWhiteSpace(paramName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(paramName));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            _parameterToValueMap.Add(paramName, value);

            return this;
        }

        public IAction Build()
        {
            var parameters = new List<string>
            {
                _commandName
            };

            foreach (var (parameter, value) in _parameterToValueMap)
            {
                parameters.Add(parameter);
                parameters.Add(value);
            }

            return new RunEvaluatorAction(parameters.ToArray());
        }
    }
}
