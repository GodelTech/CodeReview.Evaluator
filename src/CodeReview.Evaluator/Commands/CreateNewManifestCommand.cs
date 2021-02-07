using System;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Options;
using GodelTech.CodeReview.Evaluator.Services;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public class CreateNewManifestCommand : ICreateNewManifestCommand
    {
        private readonly IFileService _fileService;
        private readonly IYamlSerializer _yamlSerializer;

        public CreateNewManifestCommand(IFileService fileService, IYamlSerializer yamlSerializer)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _yamlSerializer = yamlSerializer ?? throw new ArgumentNullException(nameof(yamlSerializer));
        }
        
        public async Task<int> ExecuteAsync(NewManifestOptions options)
        {
            if (options == null) 
                throw new ArgumentNullException(nameof(options));

            var manifest = new EvaluationManifest
            {
                Queries = new ()
                {
                    ["query1"] = new()
                    {
                        Query = "SELECT * FROM Issues WHERE RuleId = @RuleId"
                    },
                    ["query2"] = new()
                    {
                        Query = "SELECT COUNT(*) FROM Issues WHERE Category = @Category"
                    }
                },
                Requests = new()
                {
                    ["inlineQuery"] = new()
                    {
                        Query = "SELECT COUNT(*) FROM Issues WHERE IssueId = @IssueId",
                        Type = RequestType.Scalar,
                        StatusRanges = new()
                        {
                            ["red"] = new StatusRange {  Start = null, End = 100 },
                            ["amber"] = new StatusRange {  Start = 100, End = 200 },
                            ["green"] = new StatusRange {  Start = 300, End = null }
                        },
                        Parameters = new()
                        {
                            ["IssueId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsValueRef = true,
                                IsInt = false,
                                IsNull = false
                            }
                        }
                    },
                    ["queryReference"] = new ()
                    {
                        QueryRef = "query2",
                        Type = RequestType.Object,
                        StatusRanges = new()
                        {
                            ["red"] = new StatusRange { Start = null, End = 100 },
                            ["amber"] = new StatusRange { Start = 100, End = 200 },
                            ["green"] = new StatusRange { Start = 300, End = null }
                        },
                        Parameters = new()
                        {
                            ["IssueId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsValueRef = true,
                                IsInt = false,
                                IsNull = false
                            }
                        }
                    },
                    ["inlineQuery"] = new()
                    {
                        Query = "SELECT TOP 1 * FROM Issues WHERE IssueId = @IssueId",
                        Type = RequestType.Collection,
                        Parameters = new ()
                        {
                            ["IssueId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsValueRef = true,
                                IsInt = true,
                                IsNull = false
                            }
                        }
                    },
                    ["queryReference"] = new ()
                    {
                        QueryRef = "query1",
                        Type = RequestType.NoResult,
                        Parameters = new()
                        {
                            ["RuleId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsValueRef = true,
                                IsInt = true,
                                IsNull = false
                            }
                        }
                    }
                }
            };

            var yaml = _yamlSerializer.Serialize(manifest);

            await _fileService.WriteAllTextAsync(options.OutputPath, yaml);
            
            return Constants.SuccessExitCode;
        }
    }
}
