using System.Collections.Generic;

namespace Validation.Service.Helpers
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public List<string> ValidationErrors { get; }

        public ValidationResult(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            ValidationErrors = errors;
        }
    }
}
