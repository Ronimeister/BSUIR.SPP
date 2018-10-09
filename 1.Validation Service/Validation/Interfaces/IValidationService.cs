using Validation.Service.Helpers;

namespace Validation.Interfaces
{
    /// <summary>
    /// Interface for all classes that implement validation service logic
    /// </summary>
    /// <typeparam name="T">Needed validation objects type</typeparam>
    public interface IValidationService <T> where T: class 
    {
        /// <summary>
        /// Method that validate <param name="value"></param> using it validation attributes
        /// </summary>
        /// <param name="value">Value need to be validated</param>
        /// <returns><see cref="ValidationResult"/> object</returns>
        ValidationResult Validate(T value);
    }
}
