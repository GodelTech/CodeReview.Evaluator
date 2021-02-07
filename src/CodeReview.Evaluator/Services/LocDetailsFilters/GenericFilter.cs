using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services.LocDetailsFilters
{
    public class GenericFilter<T> : ILocDetailsFilter
    {
        private readonly Func<FileLocDetails, T> _valueSelector;
        private readonly Func<T, bool> _predicate;

        public GenericFilter(Func<FileLocDetails, T> valueSelector, Func<T, bool> predicate)
        {
            _valueSelector = valueSelector ?? throw new ArgumentNullException(nameof(valueSelector));
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public bool IsMatch(FileLocDetails issue)
        {
            if (issue == null) throw new ArgumentNullException(nameof(issue));

            return _predicate(_valueSelector(issue));
        }
    }
}