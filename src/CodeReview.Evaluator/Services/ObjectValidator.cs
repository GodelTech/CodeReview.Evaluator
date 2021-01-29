using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ReviewItEasy.Evaluator.Services
{
    public class ObjectValidator : IObjectValidator
    {
        public ValidationResult[] Validate(object data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var validationResults = new List<ValidationResult>();

            TryValidateObject(data, new Dictionary<object, object>(), validationResults);

            return validationResults.ToArray();
        }

        private bool TryValidateObject(object obj, Dictionary<object, object> items, List<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(obj, items);

            validationResults ??= new List<ValidationResult>();
            var result = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            var properties = obj.GetType()
                .GetProperties()
                .Where(prop => prop.CanRead && prop.GetIndexParameters().Length == 0)
                .Where(prop => CanTypeBeValidated(prop.PropertyType))
                .ToList();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value == null)
                    continue;

                var valueEnumerable = value as IEnumerable<object> ?? new[] { value };
                var nestedValidationResults = new List<ValidationResult>();

                foreach (var valueToValidate in valueEnumerable)
                {
                    if (TryValidateObject(valueToValidate, items, nestedValidationResults))
                        continue;

                    result = false;

                    foreach (var validationResult in nestedValidationResults)
                    {
                        validationResults.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property.Name + '.' + x)));
                    }
                }
            }


            return result;
        }

        private static bool CanTypeBeValidated(Type type)
        {
            if (type == null)
                return false;
            if (type == typeof(string))
                return false;
            if (type.IsValueType)
                return false;

            if (!type.IsArray || !type.HasElementType)
                return true;

            var elementType = type.GetElementType();

            return CanTypeBeValidated(elementType);
        }
    }
}