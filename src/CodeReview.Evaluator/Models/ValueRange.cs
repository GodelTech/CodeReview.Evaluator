using System;
using System.Globalization;

namespace GodelTech.CodeReview.Evaluator.Models
{
    public class ValueRange
    {
        public decimal? Start { get; set; }
        public decimal? End { get; set; }

        public bool IsInRange(object value)
        {
            if (value == null)
                return false;

            try
            {
                var convertedValue =
                    (decimal) Convert.ChangeType(value, TypeCode.Decimal, CultureInfo.InvariantCulture);

                if (End.HasValue)
                {
                    if (End <= convertedValue)
                        return false;

                    if (Start == null)
                        return true;
                    
                    return Start <= convertedValue;
                }

                if (Start <= convertedValue)
                    return true;

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}