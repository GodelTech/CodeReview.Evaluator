using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class NotFilter : IIssueFilter
    {
        private readonly IIssueFilter _innerFilter;

        public NotFilter(IIssueFilter innerFilter)
        {
            _innerFilter = innerFilter ?? throw new ArgumentNullException(nameof(innerFilter));
        }
        
        public bool IsMatch(Issue issue)
        {
            if (issue == null) 
                throw new ArgumentNullException(nameof(issue));

            return !_innerFilter.IsMatch(issue);
        }
    }
}