using System;
using Validation.Interfaces;

namespace Validation.Attributes
{
    /// <summary>
    /// Attribute that validate int range
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CustomRangeAttribute : Attribute, IValidationAttribute<int>, IIntValidator
    {
        /// <summary>
        /// Minimum int value possible to validation
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// Maximum int value possible to validation
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// Validation error message
        /// </summary>
        public string ErrorMessage => Resources.Resource.CustomRangeAttributeErrorMessage;

        /// <summary>
        /// Standart .ctor for <see cref="CustomRangeAttribute"/>
        /// </summary>
        /// <param name="minimum">Minimum int value possible to validation</param>
        /// <param name="maximum">Maximum int value possible to validation</param>
        public CustomRangeAttribute(int minimum, int maximum)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException($"Invalid operators! {nameof(minimum)} can't be bigger than {maximum}!");
            }

            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Validation method
        /// </summary>
        /// <param name="value">Object need to be validated</param>
        /// <returns>The result of validation</returns>
        public bool IsValid(int value) => !(value <= Minimum || value >= Maximum);

        public bool IsValid(object value)
        {
            int validationNumber = 0;

            try
            {
                validationNumber = Convert.ToInt32(value);
            }
            catch (InvalidCastException)
            {
                return false;
            }

            return IsValid(validationNumber);
        }
    }
}
