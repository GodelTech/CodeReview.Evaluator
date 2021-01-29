namespace ReviewItEasy.Evaluator.Models
{
    public class IssueLocation
    {
        public string FilePath { get; set; }

        public IssueRegion Region { get; set; }
    }
}