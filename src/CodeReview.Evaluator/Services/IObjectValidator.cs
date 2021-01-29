using System.ComponentModel.DataAnnotations;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IObjectValidator
    {
        ValidationResult[] Validate(object data);
    }
}