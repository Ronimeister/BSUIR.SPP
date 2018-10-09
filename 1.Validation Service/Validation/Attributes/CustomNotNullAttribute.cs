using System;
using Validation.Interfaces;

namespace Validation.Attributes
{
    /// <summary>
    /// Attribute that check's some object for null equality
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomNotNullAttribute : Attribute, IValidationAttribute
    {
        /// <summary>
        /// Validation error message
        /// </summary>
        public string ErrorMessage => Resources.Resource.CustomNotNullAttributeErrorMessage;

        /// <summary>
        /// Validation method
        /// </summary>
        /// <param name="value">Object need to be validated</param>
        /// <returns>Result of validation</returns>
        public bool IsValid(object value) => value != null;
    }
}
