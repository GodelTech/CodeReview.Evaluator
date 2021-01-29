using System;
using System.Linq;
using GodelTech.CodeReview.Evaluator.Models;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class EvaluationManifestValidator : IEvaluationManifestValidator
    {
        private readonly IObjectValidator _objectValidator;
        private readonly ILogger<ScopeManifestValidator> _logger;

        public EvaluationManifestValidator(
            IObjectValidator objectValidator,
            ILogger<ScopeManifestValidator> logger)
        {
            _objectValidator = objectValidator ?? throw new ArgumentNullException(nameof(objectValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public bool IsValid(EvaluationManifest manifest)
        {
            if (manifest == null) 
                throw new ArgumentNullException(nameof(manifest));

            if (!ValidateAnnotations(manifest)) 
                return false;

            return ValidateQueryReferences(manifest) && ValidateQueryAndQueryRef(manifest);
        }

        private bool ValidateQueryAndQueryRef(EvaluationManifest manifest)
        {
            var neitherQueryNoQueryRefRequests =
                manifest.Scalars
                    .Concat(manifest.Collections)
                    .Concat(manifest.Objects)
                    .Where(x => string.IsNullOrWhiteSpace(x.Value.QueryRef) && string.IsNullOrWhiteSpace(x.Value.Query))
                    .Select(x => x.Key)
                    .ToArray();

            if (!neitherQueryNoQueryRefRequests.Any())
                return true;

            _logger.LogError("Some requests doesn't have QueryRef and Query defined: ");

            foreach (var dbRequestName in neitherQueryNoQueryRefRequests)
            {
                _logger.LogError("Request name: {requestName}", dbRequestName);
            }

            return false;
        }

        private bool ValidateAnnotations(EvaluationManifest manifest)
        {
            var validationResults = _objectValidator.Validate(manifest);

            foreach (var error in validationResults)
            {
                _logger.LogError("{@members}: {errorMessage}", error.MemberNames.ToArray(), error.ErrorMessage);
            }

            return !validationResults.Any();
        }

        private bool ValidateQueryReferences(EvaluationManifest manifest)
        {
            var allFilters =
                manifest.Scalars
                    .Concat(manifest.Collections)
                    .Concat(manifest.Objects)
                    .Select(x => x.Value)
                    .ToArray();

            var unknownQueryReferences = allFilters.Where(x =>
                !string.IsNullOrWhiteSpace(x.QueryRef)
                && !manifest.Queries.ContainsKey(x.QueryRef)).ToArray();

            if (!unknownQueryReferences.Any())
                return true;

            _logger.LogError("Unknown query references found (names are case-sensitive): ");

            foreach (var reference in unknownQueryReferences)
            {
                _logger.LogError("Unknown query reference: {reference}", reference);
            }

            return false;
        }
    }
}