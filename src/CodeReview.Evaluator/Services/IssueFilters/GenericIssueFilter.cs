using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services.IssueFilters
{
    public class GenericIssueFilter<T> : IIssueFilter
    {
        private readonly Func<Issue, T> _valueSelector;
        private readonly Func<T, bool> _predicate;

        public GenericIssueFilter(Func<Issue, T> valueSelector, Func<T, bool> predicate)
        {
            _valueSelector = valueSelector ?? throw new ArgumentNullException(nameof(valueSelector));
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public bool IsMatch(Issue issue)
        {
            if (issue == null) throw new ArgumentNullException(nameof(issue));

            return _predicate(_valueSelector(issue));
        }
    }
}