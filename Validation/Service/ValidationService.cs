using System;
using System.Collections.Generic;
using System.Reflection;
using Loggers;
using Loggers.Entities;
using Validation.Interfaces;
using Validation.Service.Helpers;

namespace Validation.Service
{
    /// <summary>
    /// Class that implements functionality of validation service
    /// </summary>
    /// <typeparam name="T">Type of object that should be validate</typeparam>
    public class ValidationService<T> : IValidationService<T> where T : class
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Standart .ctor of <see cref="ValidationService{T}"/> class
        /// </summary>
        public ValidationService()
        {
            _logger = new NLogLogger();
        }

        /// <summary>
        /// Standart .ctor of <see cref="ValidationService{T}"/> class
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> logger</param>
        /// <exception cref="ArgumentNullException">Throws when <param name="logger"></param> is equal to null</exception>
        public ValidationService(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} can't be equal to null!");
        }

        /// <summary>
        /// Public method for object validation
        /// </summary>
        /// <param name="value">Object needed to be validated</param>
        /// <returns><see cref="ValidationResult"/> object that represent the result of validation</returns>
        /// <exception cref="ArgumentNullException">Throws when <param name="value"></param> is equal to null</exception>
        public ValidationResult Validate(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{nameof(value)} can't be equal to null!");
            }

            return ValidateInner(value);
        }

        /// <summary>
        /// Private method for object validation
        /// </summary>
        /// <param name="value">Object needed to be validated</param>
        /// <returns><see cref="ValidationResult"/> object that represent the result of validation</returns>
        private ValidationResult ValidateInner(T value)
        {
            bool result = true;
            List<string> errors = new List<string>();

            //Searching for all class attributes
            foreach (var attr in ServiceHelper.GetCustomAttributes(value))
            {
                if (attr is IValidationAttribute attribute)
                {
                    ValidateAttribute(attribute, value, errors, ref result);
                }
            }

            //Searching for all class properties attributes
            foreach (var prop in ServiceHelper.GetTypeProperties(value))
            {
                if (prop.CustomAttributes != null)
                {
                    if (!ValidateProperty(prop, value, errors))
                    {
                        result = false;
                    }
                }
            }

            return new ValidationResult(result, errors);
        }

        /// <summary>
        /// Method for property validation
        /// </summary>
        /// <param name="property">Property info</param>
        /// <param name="instance">Object that contains that property</param>
        /// <param name="errorList">List of errors during the validation</param>
        /// <returns>Is property valid or not</returns>
        private bool ValidateProperty(PropertyInfo property, T instance, List<string> errorList)
        {
            bool result = true;

            foreach (var a in ServiceHelper.GetCustomAttributes(property))
            {
                if (a is IValidationAttribute attribute)
                {
                    ValidateAttribute(attribute, ServiceHelper.GetPropertyValue(property, instance), errorList, ref result);
                }
            }

            return result;
        }

        /// <summary>
        /// Method that takes attribute and use it IsValid method
        /// </summary>
        /// <param name="attribute">Needed attribute</param>
        /// <param name="value">Value needed to be validated</param>
        /// <param name="errorList">List of validation errors</param>
        /// <param name="result">The result of validation</param>
        private void ValidateAttribute(IValidationAttribute attribute, object value, List<string> errorList, ref bool result)
        {
            if (!attribute.IsValid(value))
            {
                errorList.Add(attribute.ErrorMessage);
                _logger.Warn(attribute.ErrorMessage);

                result = false;
            }
        }
    }
}
