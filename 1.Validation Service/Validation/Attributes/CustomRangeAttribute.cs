using System;
using Validation.Interfaces;

namespace Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomRangeAttribute : Attribute, IValidationAttribute
    {
        public int Minimum { get; private set; }

        public int Maximum { get; private set; }

        public string ErrorMessage { get; set; }

        public CustomRangeAttribute(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public bool IsValid(object value)
        {
            int validationNumber = value == null ? 0 : (int)value;

            if (validationNumber <= Minimum || validationNumber >= Maximum)
            {
                ErrorMessage = $"The length of the string should be in range of [{Minimum}, {Maximum}]!";

                return false;
            }

            return true;
        }
    }
}
