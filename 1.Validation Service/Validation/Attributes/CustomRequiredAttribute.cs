using System;
using Validation.Interfaces;

namespace Validation.Attributes
{
    /// <summary>
    /// Attribute for object required validation
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomRequiredAttribute : Attribute, IValidationAttribute<object>
    {
        /// <summary>
        /// Validation error
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Standart .ctor for <see cref="CustomRequiredAttribute"/>
        /// </summary>
        /// <param name="errorMessage">error message</param>
        /// <exception cref="ArgumentNullException">Throws when <param name="errorMessage"></param> is equal to null or empty</exception>
        public CustomRequiredAttribute()
        {
            ErrorMessage = Resources.Resource.CustomRequiredAttributeErrorMessage;
        }

        /// <summary>
        /// Validation method
        /// </summary>
        /// <param name="value">Object need to be validated</param>
        /// <returns>Validation result</returns>
        public bool IsValid(object value)
        {
            if (value is string stringValue)
            {
                return stringValue.Trim().Length != 0;
            }

            return value != null;
        }
    }
}
