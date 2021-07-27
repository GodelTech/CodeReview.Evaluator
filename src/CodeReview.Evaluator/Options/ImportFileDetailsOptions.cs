using CommandLine;

namespace GodelTech.CodeReview.Evaluator.Options
{
    [Verb("import-file-details", HelpText = "Import file details into new or existing database.")]
    public class ImportFileDetailsOptions
    {
        [Option('d', "details", Required = true, HelpText = "File details file path.")]
        public string FileDetailsFilePath { get; set; }

        [Option('f', "filter", Required = false, HelpText = "Filter file path.")]
        public string FilterManifestPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file path.")]
        public string OutputFilePath { get; set; }
    }
}