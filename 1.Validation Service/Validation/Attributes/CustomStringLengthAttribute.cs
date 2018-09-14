using System;
using Validation.Interfaces;

namespace Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomStringLengthAttribute : Attribute, IValidationAttribute
    {
        public int MinLength { get; set; }

        public int MaxLength { get; private set; }

        public string ErrorMessage { get; set; }

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

            MinLength = minLength;
            MaxLength = maxLength;
        }

        public bool IsValid(object value)
        {
            int length = value == null ? 0 : ((string)value).Length;

            if (length <= MinLength || length >= MaxLength)
            {
                ErrorMessage = $"The length of the string should be in range of [{MinLength}, {MaxLength}]!";

                return false;
            }

            return true;
        }
    }
}
