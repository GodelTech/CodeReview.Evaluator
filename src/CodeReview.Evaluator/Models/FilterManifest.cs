using System;
using System.ComponentModel.DataAnnotations;

namespace GodelTech.CodeReview.Evaluator.Models
{
    public class FilterManifest
    {
        [Required]
        public FilterDefinition[] RuleId { get; set; } = Array.Empty<FilterDefinition>();

        [Required]
        public FilterDefinition[] Level { get; set; } = Array.Empty<FilterDefinition>();

        [Required]
        public FilterDefinition[] Title { get; set; } = Array.Empty<FilterDefinition>();

        [Required]
        public FilterDefinition[] Message { get; set; } = Array.Empty<FilterDefinition>();

        [Required]
        public FilterDefinition[] Description { get; set; } = Array.Empty<FilterDefinition>();

        [Required]
        public FilterDefinition[] DetailsUrl { get; set; } = Array.Empty<FilterDefinition>();

        [Required]
        public FilterDefinition[] Category { get; set; } = Array.Empty<FilterDefinition>();

        [Required]
        public FilterDefinition[] Tag { get; set; } = Array.Empty<FilterDefinition>();

        [Required]
        public FilterDefinition[] FilePath { get; set; } = Array.Empty<FilterDefinition>();
    }
}