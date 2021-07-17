using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using GodelTech.CodeReview.Evaluator.Models;
using GodelTech.CodeReview.Evaluator.Services;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace GodelTech.CodeReview.Evaluator.Commands
{
    public class IssueProvider : IIssueProvider
    {
        private readonly IFileService _fileService;

        public IssueProvider(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public IEnumerable<Issue> ReadAllIssues(string issuesFilePath)
        {
            if (string.IsNullOrWhiteSpace(issuesFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(issuesFilePath));

            var file = issuesFilePath;

            using var fileStream = _fileService.OpenRead(file);
            using var zipStream = new GZipStream(fileStream, CompressionMode.Decompress);
            using var textReader = new StreamReader(zipStream);
            using var jsonReader = new JsonTextReader(textReader);

            var issues = new JsonSerializer().Deserialize<Issue[]>(jsonReader);

            foreach (var issue in issues ?? Array.Empty<Issue>())
            {
                yield return issue;
            }
        }
    }
}