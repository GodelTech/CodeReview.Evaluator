using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReviewItEasy.Evaluator.Models
{
    public class EvaluationManifest
    {
        [Required]
        public Dictionary<string, QueryManifest> Queries { get; set; } = new();

        [Required]
        public Dictionary<string, DbRequestManifest> Scalars { get; set; } = new();

        [Required]
        public Dictionary<string, DbRequestManifest> Objects { get; set; } = new();

        [Required]
        public Dictionary<string, DbRequestManifest> Collections { get; set; } = new();
    }
}