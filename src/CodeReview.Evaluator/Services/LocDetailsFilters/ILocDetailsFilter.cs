using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services.LocDetailsFilters
{
    public interface ILocDetailsFilter
    {
        bool IsMatch(FileLocDetails item);
    }
}