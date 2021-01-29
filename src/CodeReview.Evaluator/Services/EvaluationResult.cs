using System.Collections.Generic;

namespace ReviewItEasy.Evaluator.Services
{
    public class EvaluationResult
    {
        public Dictionary<string, object> Scalars { get; set; } = new();
        public Dictionary<string, object> Objects { get; set; } = new();
        public Dictionary<string, object> Collections { get; set; } = new();
    }
}