using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GodelTech.CodeReview.Evaluator.Models
{
    public class DbRequestManifest
    {
        public string Query { get; set; }
        
        [MaxLength(Constants.ValueMaxLength)]
        public string QueryRef { get; set; }

        public RequestType Type { get; set; }

        [Required]
        public Dictionary<string, ParameterManifest> Parameters { get; set; } = new();

        public Dictionary<string, StatusRange> StatusRanges { get; set; } = new();
    }
}