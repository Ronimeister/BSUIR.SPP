using Validation.Interfaces;

namespace Validation.Interfaces
{
    /// <summary>
    /// Interface that validation attributes should implement
    /// </summary>
    public interface IValidationAttribute
    {
        /// <summary>
        /// Error message of validation attribute
        /// </summary>
        string ErrorMessage { get; }
        
        bool IsValid(object value);
    }

    public interface IValidationAttribute<T> : IValidationAttribute
    {
        new string ErrorMessage { get; }

        bool IsValid(T value);
    }

    public interface IStringValidator : IValidationAttribute<string>
    {
        new bool IsValid(string value);
    }

    public interface IIntValidator : IValidationAttribute<int>
    {
        new bool IsValid(int value);
    }
}



