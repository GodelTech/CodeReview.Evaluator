using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services.LocDetailsFilters
{
    public class ConstantFilter : ILocDetailsFilter
    {
        private readonly bool _isMatch;

        public ConstantFilter(bool isMatch)
        {
            _isMatch = isMatch;
        }

        public bool IsMatch(FileLocDetails item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _isMatch;
        }
    }
}