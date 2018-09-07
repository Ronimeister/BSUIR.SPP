using System;
using System.Collections.Generic;

namespace ValidationCOre.Helpers
{
    internal class PropertyStoreItem : StoreItem
    {
        internal PropertyStoreItem(Type propertyType, IEnumerable<System.Attribute> attributes) : base(attributes)
        {
            PropertyType = propertyType;
        }

        internal Type PropertyType { get; }
    }
}
