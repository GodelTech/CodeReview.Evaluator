using System;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using GodelTech.CodeReview.Evaluator.Services;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public class ExportDbCommand : IExportDbCommand
    {
        private readonly IDatabaseService _databaseService;
        private readonly IFileService _fileService;
        private readonly IIssueService _issueService;
        private readonly IFileLocDetailsProvider _locDetailsProvider;
        private readonly ILogger<ExportDbCommand> _logger;

        public ExportDbCommand(
            IDatabaseService databaseService,
            IFileService fileService,
            IIssueService issueService,
            IFileLocDetailsProvider locDetailsProvider,
            ILogger<ExportDbCommand> logger)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _issueService = issueService ?? throw new ArgumentNullException(nameof(issueService));
            _locDetailsProvider = locDetailsProvider ?? throw new ArgumentNullException(nameof(locDetailsProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> ExecuteAsync(ExportDbOptions options)
        {
            if (options == null) 
                throw new ArgumentNullException(nameof(options));

            if (_fileService.Exists(options.OutputPath))
                _fileService.Delete(options.OutputPath);
            
            _logger.LogInformation("Creating database. File = {filePath}", options.OutputPath);
            
            await _databaseService.CreateDbAsync(options.OutputPath);

            _logger.LogInformation("Database created");
            
            _logger.LogInformation("Loading file LOC statistics...");

            var details = await _locDetailsProvider.GetDetailsAsync(options);
            await _databaseService.SaveLocDetailsAsync(options.OutputPath, details);
            
            _logger.LogInformation("LOC statistics loaded");

            _logger.LogInformation("Persisting issues...");

            var issues = await _issueService.GetIssuesAsync(options);
            
            await _databaseService.SaveIssuesAsync(
                options.OutputPath,
                issues);
            
            _logger.LogInformation("Issues persisted");

            foreach (var scriptFile in options.InitScripts ?? Array.Empty<string>())
            {
                _logger.LogInformation("Running init script. File = {filePath}", scriptFile);
                
                var content = await _fileService.ReadAllTextAsync(scriptFile);

                await _databaseService.ExecuteNonQueryAsync(options.OutputPath, content);

                _logger.LogInformation("Init script executed");
            }

            return Constants.SuccessExitCode;
        }
    }
}