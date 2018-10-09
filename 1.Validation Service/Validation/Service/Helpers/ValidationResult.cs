using System.Collections.Generic;

namespace Validation.Service.Helpers
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string[] ValidationErrors { get; }

        public ValidationResult(bool isValid, List<string> errors)
        {
            if (errors != null)
            {
                ValidationErrors = errors.ToArray();
            }

            IsValid = isValid;
        }
    }
}
