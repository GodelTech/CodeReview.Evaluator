using System.ComponentModel.DataAnnotations;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IObjectValidator
    {
        ValidationResult[] Validate(object data);
    }
}