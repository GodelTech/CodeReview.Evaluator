using System;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Options;
using GodelTech.CodeReview.Evaluator.Services;
using GodelTech.CodeReview.Evaluator.Utils;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public class ExtractMetadataCommand : IExtractMetadataCommand
    {
        private readonly IFileService _fileService;
        private readonly IOptionMetadataProvider _optionMetadataProvider;

        public ExtractMetadataCommand(
            IFileService fileService, 
            IOptionMetadataProvider optionMetadataProvider)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _optionMetadataProvider = optionMetadataProvider ?? throw new ArgumentNullException(nameof(optionMetadataProvider));
        } 

        public async Task<int> ExecuteAsync(ExtractMetadataOptions options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.OutputPath)) throw new ArgumentNullException(nameof(options.OutputPath), "Value cannot be null or whitespace.");

            var metadata = _optionMetadataProvider.GetOptionsMetadata();
            
            await _fileService.WriteAllTextAsync(options.OutputPath, metadata);
            
            return Constants.SuccessExitCode;
        }
    }
}