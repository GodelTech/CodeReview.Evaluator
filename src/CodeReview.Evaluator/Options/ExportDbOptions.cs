using CommandLine;

namespace GodelTech.CodeReview.Evaluator.Options
{
    [Verb("export-db", HelpText = "Create issue summary using provided manifest.")]
    public class ExportDbOptions : IssueProcessingOptionsBase
    {
    }
}