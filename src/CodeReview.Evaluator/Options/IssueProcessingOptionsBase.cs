using System.Collections.Generic;
using CommandLine;

namespace GodelTech.CodeReview.Evaluator.Options
{
    public abstract class IssueProcessingOptionsBase
    {
        [Option('l', "loc", Required = false, HelpText = "Lines of code statistics")]
        public string LocFilePath { get; set; }
        
        [Option('f', "folder", Required = true, HelpText = "Path to folder or file to process")]
        public string Path { get; set; }

        [Option('p', "pattern", Default = "*", Required = false, HelpText = "Search pattern used to look for files within folder")]
        public string SearchMask { get; set; }

        [Option('r', "recurse", Default = true, Required = false, HelpText = "Specifies if recurse search must be used for for files in folder")]
        public bool RecurseSearch { get; set; }

        [Option('i', "init", Required = false, Separator = ';', HelpText = "Init scripts to execute")]
        public IEnumerable<string> InitScripts { get; set; }

        [Option('s', "scope", Required = false, HelpText = "Scope of issue to analyze")]
        public string ScopeFilePath { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file path")]
        public string OutputPath { get; set; }
    }
}