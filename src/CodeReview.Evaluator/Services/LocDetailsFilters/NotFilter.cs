using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services.LocDetailsFilters
{
    public class NotFilter : ILocDetailsFilter
    {
        private readonly ILocDetailsFilter _innerFilter;

        public NotFilter(ILocDetailsFilter innerFilter)
        {
            _innerFilter = innerFilter ?? throw new ArgumentNullException(nameof(innerFilter));
        }

        public bool IsMatch(FileLocDetails item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return !_innerFilter.IsMatch(item);
        }
    }
}