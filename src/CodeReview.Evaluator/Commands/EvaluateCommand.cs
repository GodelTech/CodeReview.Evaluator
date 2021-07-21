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
        private readonly IEvaluationService _evaluationService;
        private readonly IYamlSerializer _yamlSerializer;
        private readonly IEvaluationManifestValidator _evaluationManifestValidator;
        private readonly ILogger<EvaluateCommand> _logger;

        public EvaluateCommand(
            IFileService fileService,
            IEvaluationService evaluationService,
            IYamlSerializer yamlSerializer,
            IEvaluationManifestValidator evaluationManifestValidator,
            ILogger<EvaluateCommand> logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _evaluationService = evaluationService ?? throw new ArgumentNullException(nameof(evaluationService));
            _yamlSerializer = yamlSerializer ?? throw new ArgumentNullException(nameof(yamlSerializer));
            _evaluationManifestValidator = evaluationManifestValidator ?? throw new ArgumentNullException(nameof(evaluationManifestValidator));
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

            if (!_fileService.Exists(options.DbFilePath))
            {
                _logger.LogError("Failed to validate evaluation manifest");
                return Constants.ErrorExitCode;
            }

            _logger.LogInformation("Running queries from manifest...");

            await _evaluationService.EvaluateAsync(
                evaluationScopeManifest,
                options.DbFilePath,
                options.OutputFilePath);
            
            _logger.LogInformation("Query execution completed");

            return Constants.SuccessExitCode;
        }
    }
}
