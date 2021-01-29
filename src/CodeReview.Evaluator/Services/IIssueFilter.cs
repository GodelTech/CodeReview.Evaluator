using ReviewItEasy.Evaluator.Models;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IIssueFilter
    {
        bool IsMatch(Issue issue);
    }
}