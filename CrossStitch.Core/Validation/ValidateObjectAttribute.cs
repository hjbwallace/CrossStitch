using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CrossStitch.Core.Validation
{
    public static class CustomValidator
    {
        public static bool TryValidate(object param)
        {
            var context = new ValidationContext(param, null, null);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(param, context, results, true);
        }
    }

    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public CompositeValidationResult(string errorMessage)
                    : base(errorMessage)
        {
        }

        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames)
            : base(errorMessage, memberNames)
        {
        }

        protected CompositeValidationResult(ValidationResult validationResult)
            : base(validationResult)
        {
        }

        public IEnumerable<ValidationResult> Results => _results;

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }
    }

    //http://www.technofattie.com/2011/10/05/recursive-validation-using-dataannotations.html
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Any())
            {
                var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }
}