using System;
using Validation.Interfaces;

namespace Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomClassNameAttribute : Attribute, IValidationAttribute
    {
        public string ErrorMessage => "Object's type shouldn't be equal to Person!";

        public bool IsValid(object value)
        {
            string name = value.GetType().Name;

            if (name == "Person")
            {
                return false;
            }

            return true;
        }
    }
}

//This attribute is used only for class attributes validation tests! 
    //Don't blame me for this shitty code!