using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationCOre.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomStringLengthAttribute : ValidationAttribute
    {
        public int MinLength { get; set; }

        public int MaxLength { get; private set; }

        public CustomStringLengthAttribute(int maxLength)
        {
            if (maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(maxLength)} can't be less than or equal to 0!");
            }

            MaxLength = maxLength;
        }

        public override bool IsValid(object value)
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
