using System;
using System.Collections.Generic;
using System.Linq;
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
        internal static PropertyInfo[] GetTypeProperties<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{nameof(value)} can't be equal to null!");
            }

            return value.GetType().GetProperties();
        }

        /// <summary>
        /// Method that gets <param name="property"></param> value
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="property">Needed property</param>
        /// <param name="instance">Needed object</param>
        /// <returns>Value of needed property</returns>
        internal static object GetPropertyValue<T>(this PropertyInfo property, T instance)
        {
            if (property == null)
            {
                throw new ArgumentNullException($"{nameof(property)} can't be equal to null!");
            }

            if (instance == null)
            {
                throw new ArgumentNullException($"{nameof(instance)} can't be equal to null!");
            }

            return instance.GetType().GetProperty(property.Name).GetValue(instance, null);
        } 

        /// <summary>
        /// Method that gets all custom attributes of <param name="property"></param>
        /// </summary>
        /// <param name="property">Needed property</param>
        /// <returns>Custom attributes of needed property</returns>
        internal static IEnumerable<IValidationAttribute> GetCustomAttributes(this PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException($"{nameof(property)} can't be equal to null!");
            }

            return property.GetCustomAttributes(typeof(IValidationAttribute), false).Cast<IValidationAttribute>();
        } 

        /// <summary>
        /// Method that returns all <param name="value"></param> custom attributes
        /// </summary>
        /// <param name="value">Needed object</param>
        /// <returns>Custom attributes of needed object</returns>
        internal static IEnumerable<IValidationAttribute> GetCustomAttributes(this object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{nameof(value)} can't be equal to null!");
            }

            return value.GetType().GetCustomAttributes(typeof(IValidationAttribute), false).Cast<IValidationAttribute>();
        } 
    }
}