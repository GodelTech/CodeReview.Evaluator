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
        private readonly ILocDetailsFilterFactory _filterFactory;
        private readonly IScopeManifestProvider _manifestProvider;
        private readonly ILogger<FileLocDetailsProvider> _logger;

        public FileLocDetailsProvider(
            IFileService fileService,
            IJsonSerializer jsonSerializer,
            ILocDetailsFilterFactory filterFactory,
            IScopeManifestProvider manifestProvider,
            ILogger<FileLocDetailsProvider> logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _filterFactory = filterFactory ?? throw new ArgumentNullException(nameof(filterFactory));
            _manifestProvider = manifestProvider ?? throw new ArgumentNullException(nameof(manifestProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task<FileLocDetails[]> GetDetailsAsync(ImportFileDetailsOptions options)
        {
            if (options == null) 
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(options.FileDetailsFilePath))
            {
                _logger.LogWarning("File details file was not provided");
                return Array.Empty<FileLocDetails>();
            }

            if (!_fileService.Exists(options.FileDetailsFilePath))
            {
                _logger.LogWarning("File details file was not found. File = {filePath}", options.FileDetailsFilePath);
                return Array.Empty<FileLocDetails>();
            }

            var fileContent = await _fileService.ReadAllTextAsync(options.FileDetailsFilePath);
            var data = _jsonSerializer.Deserialize<Dictionary<string, LocModel>>(fileContent);

            var items =
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

            var manifest = await _manifestProvider.GetScopeManifestAsync(options.FilterManifestPath);
            if (manifest == null)
                return items;

            var filter = _filterFactory.Create(manifest);
            
            _logger.LogInformation("Applying scope manifest. File = {filePath}", options.FilterManifestPath);

            return items.Where(filter.IsMatch).ToArray();
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
