using System;
using Validation.Interfaces;

namespace Validation.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class CustomNotNullAttribute : Attribute, IValidationAttribute
    {
        public string ErrorMessage => "Object shouldn't be equal to null!";

        public bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            return true;
        }
    }
}
