using CommandLine;

namespace GodelTech.CodeReview.Evaluator.Options
{
    [Verb("import-issues", HelpText = "Import issues into new or existing database.")]
    public class ImportIssuesOptions
    {
        [Option('i', "issues", Required = true, HelpText = "Issues file path.")]
        public string IssuesFilePath { get; set; }

        [Option('f', "filter", Required = true, HelpText = "Filter file path.")]
        public string FilterManifestPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file path.")]
        public string OutputFilePath { get; set; }
    }
}