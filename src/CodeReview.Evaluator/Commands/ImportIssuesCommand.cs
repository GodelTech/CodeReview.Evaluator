using System;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using GodelTech.CodeReview.Evaluator.Services;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public class ImportIssuesCommand : IImportIssuesCommand
    {
        private readonly IDatabaseService _databaseService;
        private readonly IFileService _fileService;
        private readonly IIssueService _issueService;
        private readonly ILogger<ImportFileDetailsCommand> _logger;

        public ImportIssuesCommand(
            IDatabaseService databaseService,
            IFileService fileService,
            IIssueService issueService,
            ILogger<ImportFileDetailsCommand> logger)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _issueService = issueService ?? throw new ArgumentNullException(nameof(issueService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> ExecuteAsync(ImportIssuesOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            await EnsureDatabaseExistsAsync(options);

            _logger.LogInformation("Persisting issues...");

            var issues = await _issueService.GetIssuesAsync(options);

            await _databaseService.SaveIssuesAsync(
                options.OutputFilePath,
                issues);

            _logger.LogInformation("Issues persisted");

            return Constants.SuccessExitCode;
        }

        private async Task EnsureDatabaseExistsAsync(ImportIssuesOptions options)
        {
            if (!_fileService.Exists(options.OutputFilePath))
            {
                _logger.LogInformation("Creating database. File = {filePath}", options.OutputFilePath);

                await _databaseService.CreateIssuesDbAsync(options.OutputFilePath);

                _logger.LogInformation("Database created");
                return;
            }

            var tableExists = await _databaseService.DoesTableExist(options.OutputFilePath, DatabaseService.IssuesTableName);

            if (tableExists)
            {
                _logger.LogWarning("Specified file exists. {TableName} table exists.", DatabaseService.IssuesTableName);
                return;
            }

            _logger.LogWarning("Specified file exists. {TableName} table was not found. Creating tables.", DatabaseService.IssuesTableName);

            await _databaseService.CreateIssuesDbAsync(options.OutputFilePath);
        }
    }
}