using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationCOre.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IntRangeAttribute : ValidationAttribute
    {
        public int Minimum { get; private set; }

        public int Maximum { get; private set; }

        public IntRangeAttribute(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override bool IsValid(object value)
        {
            int validationNumber = value == null ? 0 : (int) value;

            if (validationNumber <= Minimum || validationNumber >= Maximum)
            {
                ErrorMessage = $"The length of the string should be in range of [{Minimum}, {Maximum}]!";

                return false;
            }

            return true;
        }
    }
}
