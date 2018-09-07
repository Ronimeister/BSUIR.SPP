using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ValidationCOre.Helpers
{
    internal abstract class StoreItem
    {
        private static IEnumerable<ValidationAttribute> _emptyValidationAttributeEnumerable = new ValidationAttribute[0];

        internal StoreItem(IEnumerable<System.Attribute> attributes)
        {
            ValidationAttributes = attributes.OfType<ValidationAttribute>();
        }

        internal IEnumerable<ValidationAttribute> ValidationAttributes { get; }
    }
}
