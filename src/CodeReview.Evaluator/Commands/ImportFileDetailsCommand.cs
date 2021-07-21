using System;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using GodelTech.CodeReview.Evaluator.Services;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public class ImportFileDetailsCommand : IImportFileDetailsCommand
    {
        private readonly IDatabaseService _databaseService;
        private readonly IFileService _fileService;
        private readonly IFileLocDetailsProvider _locDetailsProvider;
        private readonly ILogger<ImportFileDetailsCommand> _logger;

        public ImportFileDetailsCommand(
            IDatabaseService databaseService,
            IFileService fileService,
            IFileLocDetailsProvider locDetailsProvider,
            ILogger<ImportFileDetailsCommand> logger)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _locDetailsProvider = locDetailsProvider ?? throw new ArgumentNullException(nameof(locDetailsProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<int> ExecuteAsync(ImportFileDetailsOptions options)
        {
            if (options == null) 
                throw new ArgumentNullException(nameof(options));

            await EnsureDatabaseExistsAsync(options);

            _logger.LogInformation("Loading file details information ...");

            var details = await _locDetailsProvider.GetDetailsAsync(options);
            await _databaseService.SaveLocDetailsAsync(options.OutputFilePath, details);
            
            _logger.LogInformation("File details information loaded");

            return Constants.SuccessExitCode;
        }

        private async Task EnsureDatabaseExistsAsync(ImportFileDetailsOptions options)
        {
            if (!_fileService.Exists(options.OutputFilePath))
            {
                _logger.LogInformation("Creating database. File = {filePath}", options.OutputFilePath);

                await _databaseService.CreateFileDetailsDbAsync(options.OutputFilePath);

                _logger.LogInformation("Database created");
                return;
            }

            var tableExists = await _databaseService.DoesTableExist(options.OutputFilePath, DatabaseService.FileDetailsTableName);

            if (tableExists)
            {
                _logger.LogWarning("Specified file exists. {TableName} table exists.", DatabaseService.FileDetailsTableName);
                return;
            }


            _logger.LogWarning("Specified file exists. {TableName} table was not found. Creating tables.", DatabaseService.FileDetailsTableName);

            await _databaseService.CreateFileDetailsDbAsync(options.OutputFilePath);
        }
    }
}