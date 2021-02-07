using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using Microsoft.Extensions.Logging;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class FileLocDetailsProvider : IFileLocDetailsProvider
    {
        private readonly IFileService _fileService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<FileLocDetailsProvider> _logger;

        public FileLocDetailsProvider(
            IFileService fileService,
            IJsonSerializer jsonSerializer,
            ILogger<FileLocDetailsProvider> logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<FileLocDetails[]> GetDetailsAsync(IssueProcessingOptionsBase options)
        {
            if (options == null) 
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(options.LocFilePath))
            {
                _logger.LogWarning("LOC statistics was not provided");
                return Array.Empty<FileLocDetails>();
            }

            if (!_fileService.Exists(options.LocFilePath))
            {
                _logger.LogWarning("LOC statistics file was not fount. File = {filePath}", options.LocFilePath);
                return Array.Empty<FileLocDetails>();
            }

            var fileContent = await _fileService.ReadAllTextAsync(options.LocFilePath);
            var data = _jsonSerializer.Deserialize<Dictionary<string, LocModel>>(fileContent);

            return
                (from item in data
                    select new FileLocDetails
                    {
                        FilePath = item.Key,
                        Blank = item.Value.Blank,
                        Code = item.Value.Code,
                        Commented = item.Value.Commented,
                        Language = item.Value.Language
                    })
                .ToArray();
        }
        
        private class LocModel
        {
            public string Language { get; set; }
            public long Blank { get; set; }
            public long Code { get; set; }
            public long Commented { get; set; }
        }
    }
}
