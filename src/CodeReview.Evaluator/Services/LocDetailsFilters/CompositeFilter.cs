using System;
using System.Linq;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services.LocDetailsFilters
{
    public class CompositeFilter : ILocDetailsFilter
    {
        private readonly bool _trueOnAnyMatch;
        private readonly ILocDetailsFilter[] _filters;

        public CompositeFilter(bool trueOnAnyMatch, params ILocDetailsFilter[] filters)
        {
            _trueOnAnyMatch = trueOnAnyMatch;
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }

        public bool IsMatch(FileLocDetails item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_trueOnAnyMatch)
                return _filters.Any(x => x.IsMatch(item));

            return _filters.All(x => x.IsMatch(item));
        }
    }
}