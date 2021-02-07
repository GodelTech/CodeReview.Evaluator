using System;
using System.Linq;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services.IssueFilters
{
    public class CompositeIssueFilter : IIssueFilter
    {
        private readonly bool _trueOnAnyMatch;
        private readonly IIssueFilter[] _filters;

        public CompositeIssueFilter(bool trueOnAnyMatch, params IIssueFilter[] filters)
        {
            _trueOnAnyMatch = trueOnAnyMatch;
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }

        public bool IsMatch(Issue issue)
        {
            if (issue == null) 
                throw new ArgumentNullException(nameof(issue));

            if (_trueOnAnyMatch)
                return _filters.Any(x => x.IsMatch(issue));

            return _filters.All(x => x.IsMatch(issue));
        }
    }
}