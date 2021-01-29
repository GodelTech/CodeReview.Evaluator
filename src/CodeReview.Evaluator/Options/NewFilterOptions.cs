using CommandLine;

namespace ReviewItEasy.Evaluator.Options
{
    [Verb("new-filter", HelpText = "Creates new filter which can be used as draft for real filter.")]
    public class NewFilterOptions
    {
        [Option('o', "output", Required = true, HelpText = "Output file path")]
        public string OutputPath { get; set; }
    }
}