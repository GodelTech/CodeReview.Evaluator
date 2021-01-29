using System;
using System.Linq;
using System.Text.RegularExpressions;
using GodelTech.CodeReview.Evaluator.Models;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class ScopeManifestValidator : IScopeManifestValidator
    {
        private readonly IObjectValidator _objectValidator;
        private readonly ILogger<ScopeManifestValidator> _logger;

        public ScopeManifestValidator(
            IObjectValidator objectValidator,
            ILogger<ScopeManifestValidator> logger)
        {
            _objectValidator = objectValidator ?? throw new ArgumentNullException(nameof(objectValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool IsValid(ScopeManifest manifest)
        {
            if (manifest == null)
                throw new ArgumentNullException(nameof(manifest));

            var validationResults = _objectValidator.Validate(manifest);

            foreach (var error in validationResults)
            {
                _logger.LogError("{@members}: {errorMessage}", error.MemberNames.ToArray(), error.ErrorMessage);
            }

            if (validationResults.Any())
                return false;

            var allFilters =
                manifest.Exclude.FilePath
                    .Concat(manifest.Exclude.Category)
                    .Concat(manifest.Exclude.Description)
                    .Concat(manifest.Exclude.DetailsUrl)
                    .Concat(manifest.Exclude.Level)
                    .Concat(manifest.Exclude.Message)
                    .Concat(manifest.Exclude.Tag)
                    .Concat(manifest.Exclude.Title)
                    .Concat(manifest.Exclude.RuleId)

                    .Concat(manifest.Include.FilePath)
                    .Concat(manifest.Include.Category)
                    .Concat(manifest.Include.Description)
                    .Concat(manifest.Include.DetailsUrl)
                    .Concat(manifest.Include.Level)
                    .Concat(manifest.Include.Message)
                    .Concat(manifest.Include.Tag)
                    .Concat(manifest.Include.Title)
                    .Concat(manifest.Include.RuleId);

            var invalidPattern = allFilters.Where(x => x.IsRegex && !IsValidRegex(x.Pattern)).ToArray();

            foreach (var pattern in invalidPattern)
            {
                _logger.LogError("Invalid RegEx patterns: {pattern}", pattern.Pattern);
            }

            return invalidPattern.Length == 0;
        }

        private static bool IsValidRegex(string pattern)
        {
            try
            {
                new Regex(pattern);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
