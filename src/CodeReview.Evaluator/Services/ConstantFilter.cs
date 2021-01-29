using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class ConstantFilter : IIssueFilter
    {
        private readonly bool _isMatch;

        public ConstantFilter(bool isMatch)
        {
            _isMatch = isMatch;
        }

        public bool IsMatch(Issue issue)
        {
            if (issue == null)
                throw new ArgumentNullException(nameof(issue));

            return _isMatch;
        }
    }
}