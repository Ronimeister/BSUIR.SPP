using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ValidationCOre.Service
{
    public interface IValidationService
    {
        bool TryValidateObject(object instance, ValidationContext context, ICollection<ValidationResult> results,
            bool validateAllProperties);
    }
}
