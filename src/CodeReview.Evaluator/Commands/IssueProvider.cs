﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using ReviewItEasy.Evaluator.Models;
using ReviewItEasy.Evaluator.Options;
using ReviewItEasy.Evaluator.Services;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace ReviewItEasy.Evaluator.Commands
{
    public class IssueProvider : IIssueProvider
    {
        private readonly IFileListResolver _fileListResolver;
        private readonly IFileService _fileService;

        public IssueProvider(IFileListResolver fileListResolver, IFileService fileService)
        {
            _fileListResolver = fileListResolver ?? throw new ArgumentNullException(nameof(fileListResolver));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }
        
        public IEnumerable<Issue> ReadAllIssues(IssueProcessingOptionsBase options)
        {
            foreach (var file in _fileListResolver.ResolveFiles(options))
            {
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
}