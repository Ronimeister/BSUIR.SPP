using Validation.Service.Helpers;

namespace Validation.Interfaces
{
    public interface IValidationService <T> where T: class 
    {
        ValidationResult Validate(T value);
    }
}
