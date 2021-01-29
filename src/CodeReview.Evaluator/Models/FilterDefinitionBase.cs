using System.ComponentModel.DataAnnotations;

namespace GodelTech.CodeReview.Evaluator.Models
{
    public class FilterDefinitionBase
    {
        [Required]
        [MaxLength(Constants.ValueMaxLength)]
        public string Pattern { get; set; }
        public bool IsRegex { get; set; }
    }
}