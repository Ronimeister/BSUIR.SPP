using System.ComponentModel.DataAnnotations;

namespace ValidationCOre.Attribute
{
    public class CustomNotNullAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                ErrorMessage = "Object shouldn't be equal to null!";

                return false;
            }

            return true;
        }
    }
}
