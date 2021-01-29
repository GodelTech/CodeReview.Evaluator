using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Commands;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class IssueService : IIssueService
    {
        private readonly IFileService _fileService;
        private readonly IYamlSerializer _yamlSerializer;
        private readonly IIssueProvider _issueProvider;
        private readonly IIssueFilterFactory _filterFactory;
        private readonly IScopeManifestValidator _scopeManifestValidator;
        private readonly ILogger<ExportDbCommand> _logger;

        public IssueService(
            IFileService fileService,
            IYamlSerializer yamlSerializer,
            IIssueProvider issueProvider,
            IIssueFilterFactory filterFactory,
            IScopeManifestValidator scopeManifestValidator,
            ILogger<ExportDbCommand> logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _yamlSerializer = yamlSerializer ?? throw new ArgumentNullException(nameof(yamlSerializer));
            _issueProvider = issueProvider ?? throw new ArgumentNullException(nameof(issueProvider));
            _filterFactory = filterFactory ?? throw new ArgumentNullException(nameof(filterFactory));
            _scopeManifestValidator = scopeManifestValidator ?? throw new ArgumentNullException(nameof(scopeManifestValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Issue>> GetIssuesAsync(IssueProcessingOptionsBase options)
        {
            var manifest = await GetScopeManifestAsync(options.ScopeFilePath);

            var allIssues = _issueProvider.ReadAllIssues(options);
            if (manifest == null)
                return allIssues;

            var filter = _filterFactory.Create(manifest);
            return allIssues.Where(filter.IsMatch);
        }

        private async Task<ScopeManifest> GetScopeManifestAsync(string scopeManifestPath)
        {
            if (string.IsNullOrWhiteSpace(scopeManifestPath))
                return null;

            _logger.LogInformation("Reading scope manifest. File = {filePath}", scopeManifestPath);

            var scopeManifestContent = await _fileService.ReadAllTextAsync(scopeManifestPath);
            var scopeManifest = _yamlSerializer.Deserialize<ScopeManifest>(scopeManifestContent);

            _logger.LogInformation("Reading scope manifest was read");

            _logger.LogInformation("Validating scope manifest...");

            if (!_scopeManifestValidator.IsValid(scopeManifest))
            {
                _logger.LogError("Failed to validate scope manifest");

                throw new EvaluateException("Failed to validate scope manifest");
            }

            _logger.LogInformation("Manifest validated");

            return scopeManifest;
        }
    }
}