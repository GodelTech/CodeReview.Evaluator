using CommandLine;

namespace ReviewItEasy.Evaluator.Options
{
    [Verb("export-db", HelpText = "Create issue summary using provided manifest.")]
    public class ExportDbOptions : IssueProcessingOptionsBase
    {
    }
}