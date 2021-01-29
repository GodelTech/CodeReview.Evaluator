using System.ComponentModel.DataAnnotations;

namespace ReviewItEasy.Evaluator.Models
{
    public class ScopeManifest
    {
        [Required]
        public FilterManifest Include { get; set; } = new();
        
        [Required]
        public FilterManifest Exclude { get; set; } = new();
    }
}
