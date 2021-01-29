using CommandLine;

namespace ReviewItEasy.Evaluator.Options
{
    [Verb("new-manifest", HelpText = "Creates new manifest which can be used as draft manifest.")]
    public class NewManifestOptions
    {
        [Option('o', "output", Required = true, HelpText = "Output file path")]
        public string OutputPath { get; set; }
    }
}