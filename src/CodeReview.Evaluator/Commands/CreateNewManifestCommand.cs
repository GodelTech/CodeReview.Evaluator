﻿using System;
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
                        Query = "SELECT * FROM Issues WHERE RuleId = $RuleId"
                    },
                    ["query2"] = new()
                    {
                        Query = "SELECT COUNT(*) FROM Issues WHERE Category = $Category"
                    }
                },
                Requests = new()
                {
                    ["inlineQuery"] = new()
                    {
                        Query = "SELECT COUNT(*) FROM Issues WHERE Id = $IssueId",
                        Type = RequestType.Scalar,
                        AddToOutput = true,
                        Ranges = new()
                        {
                            ["red"] = new ValueRange {  Start = null, End = 100 },
                            ["amber"] = new ValueRange {  Start = 100, End = 200 },
                            ["green"] = new ValueRange {  Start = 300, End = null }
                        },
                        Parameters = new()
                        {
                            ["IssueId"] = new ParameterManifest
                            {
                                Value = "123",
                                IsValueRef = true,
                                IsInt = true,
                                IsNull = true
                            }
                        }
                    },
                    ["queryReference"] = new ()
                    {
                        QueryRef = "query2",
                        Type = RequestType.Object,
                        Ranges = null,
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
                    }
                }
            };

            var yaml = _yamlSerializer.Serialize(manifest);

            await _fileService.WriteAllTextAsync(options.OutputPath, yaml);
            
            return Constants.SuccessExitCode;
        }
    }
}
