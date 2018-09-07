using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ValidationCOre.Helpers
{
    internal class AttributeStore
    {
        private Dictionary<Type, TypeStoreItem> _typeStoreItems = new Dictionary<Type, TypeStoreItem>();

        internal IEnumerable<ValidationAttribute> GetPropertyValidationAttributes(ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException($"{nameof(validationContext)} can't be equal to null!");
            }

            TypeStoreItem typeItem = GetTypeStoreItem(validationContext.ObjectType);
            PropertyStoreItem item = typeItem.GetPropertyStoreItem(validationContext.MemberName);

            return item.ValidationAttributes;
        }

        internal IEnumerable<ValidationAttribute> GetTypeValidationAttributes(ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException($"{nameof(validationContext)} can't be equal to null!");
            }

            TypeStoreItem item = GetTypeStoreItem(validationContext.ObjectType);
            return item.ValidationAttributes;
        }

        private TypeStoreItem GetTypeStoreItem(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException($"{nameof(type)} can't be equal to null!");
            }

            lock (_typeStoreItems)
            {
                if (!_typeStoreItems.TryGetValue(type, out TypeStoreItem item))
                {
                    IEnumerable<System.Attribute> attributes = TypeDescriptor.GetAttributes(type).Cast<System.Attribute>();//не находит атрибуты

                    item = new TypeStoreItem(type, attributes);
                    _typeStoreItems[type] = item;
                }
                return item;
            }
        }
    }
}
