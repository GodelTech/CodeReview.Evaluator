using System;
using System.Data;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class ScopeManifestProvider : IScopeManifestProvider
    {
        private readonly IFileService _fileService;
        private readonly IYamlSerializer _yamlSerializer;
        private readonly IScopeManifestValidator _scopeManifestValidator;

        public ScopeManifestProvider(
            IFileService fileService,
            IYamlSerializer yamlSerializer,
            IScopeManifestValidator scopeManifestValidator)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _yamlSerializer = yamlSerializer ?? throw new ArgumentNullException(nameof(yamlSerializer));
            _scopeManifestValidator = scopeManifestValidator ?? throw new ArgumentNullException(nameof(scopeManifestValidator));
        }
        
        public async Task<ScopeManifest> GetScopeManifestAsync(string scopeManifestPath)
        {
            if (string.IsNullOrWhiteSpace(scopeManifestPath))
                return null;

            var scopeManifestContent = await _fileService.ReadAllTextAsync(scopeManifestPath);
            var scopeManifest = _yamlSerializer.Deserialize<ScopeManifest>(scopeManifestContent);

            if (!_scopeManifestValidator.IsValid(scopeManifest))
                throw new EvaluateException("Failed to validate scope manifest");

            return scopeManifest;
        }
    }
}