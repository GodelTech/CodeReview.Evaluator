using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class NotLocDetailsFilter : ILocDetailsFilter
    {
        private readonly ILocDetailsFilter _innerFilter;

        public NotLocDetailsFilter(ILocDetailsFilter innerFilter)
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