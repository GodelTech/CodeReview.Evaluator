using System;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using GodelTech.CodeReview.Evaluator.Services;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public class EvaluateCommand : IEvaluateCommand
    {
        private readonly IFileService _fileService;
        private readonly IDatabaseService _databaseService;
        private readonly IEvaluationService _evaluationService;
        private readonly IYamlSerializer _yamlSerializer;
        private readonly IEvaluationManifestValidator _evaluationManifestValidator;
        private readonly IIssueService _issueService;
        private readonly IFileLocDetailsProvider _locDetailsProvider;
        private readonly ILogger<EvaluateCommand> _logger;

        public EvaluateCommand(
            IFileService fileService,
            IDatabaseService databaseService,
            IEvaluationService evaluationService,
            IYamlSerializer yamlSerializer,
            IEvaluationManifestValidator evaluationManifestValidator,
            IIssueService issueService,
            IFileLocDetailsProvider locDetailsProvider,
            ILogger<EvaluateCommand> logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _evaluationService = evaluationService ?? throw new ArgumentNullException(nameof(evaluationService));
            _yamlSerializer = yamlSerializer ?? throw new ArgumentNullException(nameof(yamlSerializer));
            _evaluationManifestValidator = evaluationManifestValidator ?? throw new ArgumentNullException(nameof(evaluationManifestValidator));
            _issueService = issueService ?? throw new ArgumentNullException(nameof(issueService));
            _locDetailsProvider = locDetailsProvider ?? throw new ArgumentNullException(nameof(locDetailsProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> ExecuteAsync(EvaluateOptions options)
        {
            if (options == null) 
                throw new ArgumentNullException(nameof(options));

            _logger.LogInformation("Reading evaluation manifest. File = {filePath}", options.ManifestFilePath);

            var evaluationManifestContent = await _fileService.ReadAllTextAsync(options.ManifestFilePath);
            var evaluationScopeManifest = _yamlSerializer.Deserialize<EvaluationManifest>(evaluationManifestContent);

            _logger.LogInformation("Reading evaluation manifest was read");

            _logger.LogInformation("Validating evaluation manifest...");

            if (!_evaluationManifestValidator.IsValid(evaluationScopeManifest))
            {
                _logger.LogError("Failed to validate evaluation manifest");
                return Constants.ErrorExitCode;
            }

            _logger.LogInformation("Manifest validated");

            var issues = await _issueService.GetIssuesAsync(options);

            using var file = _fileService.CreateTempFile();

            _logger.LogInformation("Creating database...");

            await _databaseService.CreateDbAsync(file.FilePath);

            _logger.LogInformation("Database created");

            _logger.LogInformation("Loading file LOC statistics...");

            var details = await _locDetailsProvider.GetDetailsAsync(options);
            await _databaseService.SaveLocDetailsAsync(file.FilePath, details);

            _logger.LogInformation("LOC statistics loaded");

            _logger.LogInformation("Persisting issues...");

            await _databaseService.SaveIssuesAsync(file.FilePath, issues);

            _logger.LogInformation("Issues persisted");

            foreach (var scriptFile in options.InitScripts ?? Array.Empty<string>())
            {
                _logger.LogInformation("Running init script. File = {filePath}", scriptFile);

                var content = await _fileService.ReadAllTextAsync(scriptFile);

                await _databaseService.ExecuteNonQueryAsync(file.FilePath, content);

                _logger.LogInformation("Init script executed");
            }
            
            _logger.LogInformation("Running queries from manifest...");

            await _evaluationService.EvaluateAsync(
                evaluationScopeManifest,
                file.FilePath,
                options.OutputPath);
            
            _logger.LogInformation("Query execution completed");

            return Constants.SuccessExitCode;
        }
    }
}
