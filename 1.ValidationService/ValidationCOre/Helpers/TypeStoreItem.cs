using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ValidationCOre.Helpers
{
    internal class TypeStoreItem : StoreItem
    {
        #region Private fields
        private readonly object _syncRoot = new object();
        private readonly Type _type;
        private Dictionary<string, PropertyStoreItem> _propertyStoreItems;
        #endregion

        #region Internal methods
        internal TypeStoreItem(Type type, IEnumerable<System.Attribute> attributes) : base(attributes)
        {
            _type = type;
        }

        internal PropertyStoreItem GetPropertyStoreItem(string propertyName)
        {
            if (!TryGetPropertyStoreItem(propertyName, out PropertyStoreItem item))
            {
                throw new ArgumentException($"There is no attributes for {nameof(propertyName)}!");
            }
            return item;
        }

        internal bool TryGetPropertyStoreItem(string propertyName, out PropertyStoreItem item)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            if (_propertyStoreItems == null)
            {
                lock (_syncRoot)
                {
                    if (_propertyStoreItems == null)
                    {
                        _propertyStoreItems = CreatePropertyStoreItems();
                    }
                }
            }
            if (!_propertyStoreItems.TryGetValue(propertyName, out item))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Private methods
        private Dictionary<string, PropertyStoreItem> CreatePropertyStoreItems()
        {
            Dictionary<string, PropertyStoreItem> propertyStoreItems = new Dictionary<string, PropertyStoreItem>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(_type);
            foreach (PropertyDescriptor property in properties)
            {
                PropertyStoreItem item = new PropertyStoreItem(property.PropertyType, GetExplicitAttributes(property).Cast<System.Attribute>());
                propertyStoreItems[property.Name] = item;
            }

            return propertyStoreItems;
        }

        public static AttributeCollection GetExplicitAttributes(PropertyDescriptor propertyDescriptor)
        {
            List<System.Attribute> attributes = new List<System.Attribute>(propertyDescriptor.Attributes.Cast<System.Attribute>());
            IEnumerable<System.Attribute> typeAttributes = TypeDescriptor.GetAttributes(propertyDescriptor.PropertyType).Cast<System.Attribute>();
            bool removedAttribute = false;
            foreach (System.Attribute attr in typeAttributes)
            {
                for (int i = attributes.Count - 1; i >= 0; --i)
                {
                    if (ReferenceEquals(attr, attributes[i]))
                    {
                        attributes.RemoveAt(i);
                        removedAttribute = true;
                    }
                }
            }
            return removedAttribute ? new AttributeCollection(attributes.ToArray()) : propertyDescriptor.Attributes;
        }
        #endregion
    }
}
