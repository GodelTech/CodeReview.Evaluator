using CommandLine;

namespace GodelTech.CodeReview.Evaluator.Options
{
    [Verb("evaluate", HelpText = "Executes SQL queries against provided database using specified manifest.")]
    public class EvaluateOptions
    {
        [Option('d', "db", Required = true, HelpText = "Database file path.")]
        public string DbFilePath { get; set; }

        [Option('m', "manifest", Required = true, HelpText = "Evaluation manifest file path.")]
        public string FilterManifestPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file path for JSON document.")]
        public string OutputFilePath { get; set; }
    }
}