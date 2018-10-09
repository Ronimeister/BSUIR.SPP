using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Loggers;
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

        private List<string> _errors;

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

            //Searching for all class attributes
            value.GetCustomAttributes().ToList().ForEach(attr => ValidateAttribute(attr, value, ref result));

            //Searching for all class properties attributes
            value.GetTypeProperties().ToList().ForEach(prop => ValidatePropertyInner(prop, value, ref result));

            return CreateValidationResult(result, _errors);
        }

        /// <summary>
        /// Method for property validation
        /// </summary>
        /// <param name="property">Property info</param>
        /// <param name="instance">Object that contains that property</param>
        /// <param name="errorList">List of errors during the validation</param>
        /// <returns>Is property valid or not</returns>
        private bool ValidateProperty(PropertyInfo property, T instance)
        {
            bool result = true;

            property.GetCustomAttributes().ToList().ForEach(a => ValidateAttribute(a, property.GetPropertyValue(instance), ref result));

            return result;
        }

        private void ValidatePropertyInner(PropertyInfo prop, T value, ref bool result)
        {
            if (prop.CustomAttributes != null)
            {
                if (!ValidateProperty(prop, value))
                {
                    result = false;
                }
            }
        }

        /// <summary>
        /// Method that takes attribute and use it IsValid method
        /// </summary>
        /// <param name="attribute">Needed attribute</param>
        /// <param name="value">Value needed to be validated</param>
        /// <param name="errorList">List of validation errors</param>
        /// <param name="result">The result of validation</param>
        private void ValidateAttribute(IValidationAttribute attribute, object value, ref bool result)
        {
            if (!attribute.IsValid(value))
            {
                if (_errors == null)
                {
                    _errors = new List<string>();
                }

                _errors.Add(attribute.ErrorMessage);
                _logger.Warn(attribute.ErrorMessage);

                result = false;
            }
        }

        private ValidationResult CreateValidationResult(bool result, List<string> errors)
        {
            ValidationResult validationResult = new ValidationResult(result, _errors);
            if (_errors != null)
            {
                _errors.Clear();
            }

            return validationResult;
        }
    }
}