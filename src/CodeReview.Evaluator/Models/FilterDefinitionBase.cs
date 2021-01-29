using System.ComponentModel.DataAnnotations;

namespace ReviewItEasy.Evaluator.Models
{
    public class FilterDefinitionBase
    {
        [Required]
        [MaxLength(Constants.ValueMaxLength)]
        public string Pattern { get; set; }
        public bool IsRegex { get; set; }
    }
}