using System;
using System.Threading.Tasks;
using ReviewItEasy.Evaluator.Models;
using ReviewItEasy.Evaluator.Options;
using ReviewItEasy.Evaluator.Services;

namespace ReviewItEasy.Evaluator.Commands
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
                Scalars = new()
                {
                    ["inlineQuery"] = new()
                    {
                        Query = "SELECT COUNT(*) FROM Issues WHERE IssueId = @IssueId",
                        Parameters = new()
                        {
                            ["IssueId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsInt = false,
                                IsNull = false
                            }
                        }
                    },
                    ["queryReference"] = new ()
                    {
                        QueryRef = "query2",
                        Parameters = new()
                        {
                            ["IssueId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsInt = false,
                                IsNull = false
                            }
                        }
                    }
                },
                Objects = new ()
                {
                    ["inlineQuery"] = new()
                    {
                        Query = "SELECT TOP 1 * FROM Issues WHERE IssueId = @IssueId",
                        Parameters = new ()
                        {
                            ["IssueId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsInt = true,
                                IsNull = false
                            }
                        }
                    },
                    ["queryReference"] = new ()
                    {
                        QueryRef = "query1",
                        Parameters = new()
                        {
                            ["RuleId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsInt = true,
                                IsNull = false
                            }
                        }
                    }
                },
                Collections = new()
                {
                    ["inlineQuery"] = new()
                    {
                        Query = "SELECT * FROM Issues WHERE IssueId = @IssueId",
                        Parameters = new()
                        {
                            ["IssueId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsInt = true,
                                IsNull = false
                            }
                        }
                    },
                    ["queryReference"] = new ()
                    {
                        QueryRef = "query1",
                        Parameters = new()
                        {
                            ["RuleId"] = new ParameterManifest
                            {
                                Value = "123",
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
