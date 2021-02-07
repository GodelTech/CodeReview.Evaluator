using GodelTech.CodeReview.Evaluator.Models;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface ILocDetailsFilter
    {
        bool IsMatch(FileLocDetails item);
    }
}