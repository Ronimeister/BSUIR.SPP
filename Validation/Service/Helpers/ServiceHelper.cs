using System.Reflection;
using Validation.Interfaces;

namespace Validation.Service
{
    /// <summary>
    /// Helper class that provides some additional functionality to <see cref="ValidationService{T}"/>
    /// </summary>
    internal static class ServiceHelper
    {
        /// <summary>
        /// Method that gets all properties of <param name="value"></param>
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="value">Needed object</param>
        /// <returns>All <param name="value"></param> properties</returns>
        internal static PropertyInfo[] GetTypeProperties<T>(T value)
            => value.GetType().GetProperties();

        /// <summary>
        /// Method that gets <param name="property"></param> value
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="property">Needed property</param>
        /// <param name="instance">Needed object</param>
        /// <returns>Value of needed property</returns>
        internal static object GetPropertyValue<T>(PropertyInfo property, T instance)
            => instance.GetType().GetProperty(property.Name).GetValue(instance, null);

        /// <summary>
        /// Method that gets all custom attributes of <param name="property"></param>
        /// </summary>
        /// <param name="property">Needed property</param>
        /// <returns>Custom attributes of needed property</returns>
        internal static object[] GetCustomAttributes(PropertyInfo property)
            => property.GetCustomAttributes(typeof(IValidationAttribute), false);

        /// <summary>
        /// Method that returns all <param name="value"></param> custom attributes
        /// </summary>
        /// <param name="value">Needed object</param>
        /// <returns>Custom attributes of needed object</returns>
        internal static object[] GetCustomAttributes(object value)
            => value.GetType().GetCustomAttributes(typeof(IValidationAttribute), false);
    }
}
