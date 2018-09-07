using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationCOre.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomRequiredAttribute : ValidationAttribute
    {
        public bool AllowEmptyString { get; set; }

        public CustomRequiredAttribute()
            : base(() => $"Member of a class with '[{nameof(RequiredAttribute)}]' attribute can't be epmty or equal to null!"){}

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            
            if (value is string stringValue && !AllowEmptyString)
            {
                return stringValue.Trim().Length != 0;
            }

            return true;
        }
    }
}
