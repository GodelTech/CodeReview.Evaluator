namespace GodelTech.CodeReview.Evaluator.Models
{
    public class FileLocDetails
    {
        public string FilePath { get; set; }
        public string Language { get; set; }
        public long Blank { get; set; }
        public long Code { get; set; }
        public long Commented { get; set; }
    }
}
