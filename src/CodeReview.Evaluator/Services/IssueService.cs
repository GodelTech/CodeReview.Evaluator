using System;
using System.Collections.Generic;
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
        private readonly IIssueProvider _issueProvider;
        private readonly IIssueFilterFactory _filterFactory;
        private readonly IScopeManifestProvider _manifestProvider;
        private readonly ILogger<ImportFileDetailsCommand> _logger;

        public IssueService(
            IIssueProvider issueProvider,
            IIssueFilterFactory filterFactory,
            IScopeManifestProvider manifestProvider,
            ILogger<ImportFileDetailsCommand> logger)
        {
            _issueProvider = issueProvider ?? throw new ArgumentNullException(nameof(issueProvider));
            _filterFactory = filterFactory ?? throw new ArgumentNullException(nameof(filterFactory));
            _manifestProvider = manifestProvider ?? throw new ArgumentNullException(nameof(manifestProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Issue>> GetIssuesAsync(ImportIssuesOptions options)
        {
            var manifest = await _manifestProvider.GetScopeManifestAsync(options.FilterManifestPath);

            var allIssues = _issueProvider.ReadAllIssues(options.IssuesFilePath);
            if (manifest == null)
                return allIssues;

            _logger.LogInformation("Applying scope manifest. File = {filePath}", options.FilterManifestPath);
            
            var filter = _filterFactory.Create(manifest);
            return allIssues.Where(filter.IsMatch);
        }
    }
}