﻿using CodeReview.Evaluator.IntegrationTests.Utils;
using StoryLine.Contracts;

namespace CodeReview.Evaluator.IntegrationTests.Actions
{
    public class Evaluate : IActionBuilder
    {
        private static readonly ResourceNameResolver ResourceNameResolver = new();
        private static readonly string SqliteDatabaseResourceName = ResourceNameResolver.FindResourceInResourceFolder("issues.db");

        private string _manifestResourceName;

        public Evaluate WithManifestFromResources(string suffix = ".manifest.")
        {
            _manifestResourceName = ResourceNameResolver.FindResultResourceNameByMethod(suffix);

            return this;
        }

        public IAction Build()
        {
            return new EvaluateAction(
                SqliteDatabaseResourceName,
                _manifestResourceName);
        }
    }
}
