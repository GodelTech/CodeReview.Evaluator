using CommandLine;

namespace GodelTech.CodeReview.Evaluator.Options
{
    [Verb("evaluate", HelpText = "Create issue summary using provided manifest.")]
    public class EvaluateOptions : IssueProcessingOptionsBase
    {
        [Option('m', "manifest", Required = true, HelpText = "Manifest file path")]
        public string ManifestFilePath { get; set; }
    }
}