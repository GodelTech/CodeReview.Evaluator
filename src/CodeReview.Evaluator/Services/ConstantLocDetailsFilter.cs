using System;
using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class ConstantLocDetailsFilter : ILocDetailsFilter
    {
        private readonly bool _isMatch;

        public ConstantLocDetailsFilter(bool isMatch)
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