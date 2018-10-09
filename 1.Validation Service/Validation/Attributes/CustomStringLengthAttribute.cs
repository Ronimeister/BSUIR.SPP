using System;
using Validation.Interfaces;

namespace Validation.Attributes
{
    /// <summary>
    /// Attribute for string length validation
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CustomStringLengthAttribute : Attribute, IValidationAttribute<string>, IStringValidator
    {
        /// <summary>
        /// Minimum string length possible to validation
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// Maximum string length possible to validation
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Validation error
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Standart .ctor for <see cref="CustomStringLengthAttribute"/>
        /// </summary>
        /// <param name="minLength">Minimum string length possible to validation</param>
        /// <param name="maxLength">Maximum string length possible to validation</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when:
        /// 1) <param name="minLength"></param> is lower or equal to 0
        /// 2) <param name="maxLength"></param> is lower or equal to 0
        /// </exception>
        /// <exception cref="ArgumentException">Throws when <param name="minLength"></param> is bigger <param name="maxLength"></param></exception>
        public CustomStringLengthAttribute(int minLength, int maxLength)
        {
            if (minLength <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(minLength)} can't be less than or equal to 0!");
            }

            if (maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(maxLength)} can't be less than or equal to 0!");
            }

            if (minLength > maxLength)
            {
                throw new ArgumentException($"{nameof(minLength)} can't be bigger than {nameof(maxLength)}!");
            }

            MinLength = minLength;
            MaxLength = maxLength;
            ErrorMessage = Resources.Resource.CustomStringLengthAttributeAttributeErrorMessage;
        }

        /// <summary>
        /// Validation method
        /// </summary>
        /// <param name="value">Object need to be validated</param>
        /// <returns>Validation result</returns>
        public bool IsValid(object value)
        {
            string result = (string) value;

            if (result == null)
            {
                return false;
            }

            return IsValid(result);
        }

        public bool IsValid(string value) => (value.Length >= MinLength && value.Length <= MaxLength);
    }
}

//fluent assertions 2 октяьря, 12-00, купревича 1-1, взять паспорт onboarding centre