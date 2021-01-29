using System;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using GodelTech.CodeReview.Evaluator.Services;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public class CreateNewFilterCommand : ICreateNewFilterCommand
    {
        private readonly IFileService _fileService;
        private readonly IYamlSerializer _yamlSerializer;

        public CreateNewFilterCommand(IFileService fileService, IYamlSerializer yamlSerializer)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _yamlSerializer = yamlSerializer ?? throw new ArgumentNullException(nameof(yamlSerializer));
        }

        public async Task<int> ExecuteAsync(NewFilterOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var manifest = new ScopeManifest
            {
                Include = new()
                {
                    FilePath = new FilterDefinition[]
                        {
                            new() {Pattern = "Value"},
                            new() {Pattern = "Value", IsRegex = true}
                        },
                    Category = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Description = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    DetailsUrl = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    RuleId = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Level = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Message = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Tag = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Title = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    }
                },
                Exclude = new()
                {
                    FilePath = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Category = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Description = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    DetailsUrl = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    RuleId = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Level = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Message = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Tag = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    },
                    Title = new FilterDefinition[]
                    {
                        new() {Pattern = "Value"},
                        new() {Pattern = "Value", IsRegex = true}
                    }
                }
            };

            var yaml = _yamlSerializer.Serialize(manifest);

            await _fileService.WriteAllTextAsync(options.OutputPath, yaml);

            return Constants.SuccessExitCode;
        }
    }
}