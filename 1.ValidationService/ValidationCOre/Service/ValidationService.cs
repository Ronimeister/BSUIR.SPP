using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Logger;
using ValidationCOre.Helpers;

namespace ValidationCOre.Service
{
    public class ValidationService : IValidationService
    {
        #region Private fields
        private readonly AttributeStore _store = new AttributeStore();
        private readonly ILogger _logger;

        private static ValidationService _instance;
        private static readonly object _locker = new object();
        #endregion

        #region .ctors
        private ValidationService(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} can't be equal to null!");
        }
        #endregion

        #region Public API
        public static ValidationService Instance(ILogger logger)
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new ValidationService(logger);
                    }
                }
            }

            return _instance;
        }

        public bool TryValidateObject(object instance, ValidationContext context, ICollection<ValidationResult> results, bool validateAllProperties)
        {
            if (instance == null)
            {
                throw new ArgumentNullException($"{nameof(instance)} can't be equal to null!");
            }

            if (instance != context.ObjectInstance)
            {
                throw new ArgumentException($"{nameof(instance)} must match validation context instance!");
            }

            if (context == null)
            {
                throw new ArgumentNullException($"{nameof(context)} can't be equal to null!");
            }

            if (results == null)
            {
                throw new ArgumentNullException($"{nameof(results)} can't be equal to null!");
            }

            return TryValidateObjectInner(instance, context, results, validateAllProperties);
        }
        #endregion

        #region Private methods
        private bool TryValidateObjectInner(object instance, ValidationContext context,
            ICollection<ValidationResult> results, bool validateAllProperties)
        {
            bool result = true;
            bool breakOnFirstError = (results == null);

            foreach (var err in GetObjectValidationErrors(instance, context, validateAllProperties, breakOnFirstError))
            {
                result = false;

                _logger.Warn($"{err.ValidationResult}");
                results.Add(err.ValidationResult);
            }

            return result;
        }

        private IEnumerable<ValidationError> GetObjectValidationErrors(object instance, ValidationContext context,
            bool validateAllProperties, bool breakOnFirstError)
        {
            List<ValidationError> errors = new List<ValidationError>();
            errors.AddRange(GetObjectPropertyValidationErrors(instance, context, validateAllProperties, breakOnFirstError));

            if (errors.Any())
            {
                return errors;
            }

            IEnumerable<ValidationAttribute> attributes = _store.GetTypeValidationAttributes(context);
            errors.AddRange(GetValidationErrors(instance, context, attributes, breakOnFirstError));

            if (errors.Any())
            {
                return errors;
            }

            if (instance is IValidatableObject validatable)
            {
                IEnumerable<ValidationResult> results = validatable.Validate(context);

                foreach (ValidationResult result in results.Where(r => r != ValidationResult.Success))
                {
                    errors.Add(new ValidationError(null, instance, result));
                }
            }

            return errors;
        }

        private IEnumerable<ValidationError> GetObjectPropertyValidationErrors(object instance,
            ValidationContext context, bool validateAllProperties, bool breakOnFirstError)
        {
            ICollection<KeyValuePair<ValidationContext, object>> properties = GetPropertyValues(instance, context);// неправильные проперти
            List<ValidationError> errors = new List<ValidationError>();

            foreach (KeyValuePair<ValidationContext, object> property in properties)
            {
                IEnumerable<ValidationAttribute> attributes = _store.GetPropertyValidationAttributes(property.Key);
                if (validateAllProperties)
                {
                    errors.AddRange(GetValidationErrors(property.Value, property.Key, attributes, breakOnFirstError));
                }
                else
                {
                    if (attributes.FirstOrDefault(a => a is RequiredAttribute) is RequiredAttribute reqAttr)
                    {
                        ValidationResult validationResult = reqAttr.GetValidationResult(property.Value, property.Key);
                        if (validationResult != ValidationResult.Success)
                        {
                            errors.Add(new ValidationError(reqAttr, property.Value, validationResult));
                        }
                    }
                }

                if (breakOnFirstError && errors.Any())
                {
                    break;
                }
            }

            return errors;
        }

        private ICollection<KeyValuePair<ValidationContext, object>> GetPropertyValues(object instance, ValidationContext context)
        {
            //Берём все свойства объекта instance
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(instance);

            //Создаем лист пар "ключ-значение" размерностью равной количеству свойств объекта instance
            List<KeyValuePair<ValidationContext, object>> items = new List<KeyValuePair<ValidationContext, object>>(properties.Count);

            //Начинаем итерироваться по коллекции свойств объекта
            foreach (PropertyDescriptor property in properties)
            {
                //Создаем темп-переменную для контекста
                ValidationContext valContext = CreateContext(instance, context);
                valContext.MemberName = property.Name;

                if (_store.GetPropertyValidationAttributes(valContext).Any())
                {
                    items.Add(new KeyValuePair<ValidationContext, object>(valContext, property.GetValue(instance)));
                }
            }

            return items;
        }

        private ValidationContext CreateContext(object instance, ValidationContext context)
        {
            ValidationContext cntx = new ValidationContext(instance, context, context.Items);

            return cntx;
        }

        private IEnumerable<ValidationError> GetValidationErrors(object value, ValidationContext validationContext,
            IEnumerable<ValidationAttribute> attributes, bool breakOnFirstError)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException($"{nameof(validationContext)} can't be equal to null!");
            }

            List<ValidationError> errors = new List<ValidationError>();
            ValidationError validationError;

            RequiredAttribute required = attributes.FirstOrDefault(a => a is RequiredAttribute) as RequiredAttribute;
            if (required != null)
            {
                if (!TryValidate(value, validationContext, required, out validationError))
                {
                    errors.Add(validationError);
                    return errors;
                }
            }

            foreach (ValidationAttribute attr in attributes)
            {
                if (attr != required)
                {
                    if (!TryValidate(value, validationContext, attr, out validationError))
                    {
                        errors.Add(validationError);

                        if (breakOnFirstError)
                        {
                            break;
                        }
                    }
                }
            }

            return errors;
        }

        private bool TryValidate(object value, ValidationContext validationContext, ValidationAttribute attribute, out ValidationError validationError)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException($"{nameof(validationContext)} can't be equal to null!");
            }

            ValidationResult validationResult = attribute.GetValidationResult(value, validationContext);
            if (validationResult != ValidationResult.Success)
            {
                validationError = new ValidationError(attribute, value, validationResult);

                return false;
            }

            validationError = null;

            return true;
        }
        #endregion

        #region Helpers
        private class ValidationError
        {
            internal ValidationError(ValidationAttribute attribute, object value, ValidationResult validationResult)
            {
                Attribute = attribute;
                Value = value;
                ValidationResult = validationResult;
            }

            internal ValidationAttribute Attribute { get; set; }
            internal object Value { get; set; }
            internal ValidationResult ValidationResult { get; set; }

            internal void ThrowValidationException() => throw new ValidationException(ValidationResult, Attribute, Value);
        }
        #endregion
    }
}
