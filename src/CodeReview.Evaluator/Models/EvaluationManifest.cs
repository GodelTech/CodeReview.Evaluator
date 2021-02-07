using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GodelTech.CodeReview.Evaluator.Models
{
    public class EvaluationManifest
    {
        [Required]
        public Dictionary<string, QueryManifest> Queries { get; set; } = new();

        public Dictionary<string, DbRequestManifest> Requests { get; set; } = new();
    }
}