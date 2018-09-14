using System;
using NUnit.Framework;
using Validation.Attributes;
using Validation.Service;
using Validation.Service.Helpers;

namespace Validation.Tests
{
    [TestFixture]
    public class ValidationServiceTests
    {
        [Test]
        public void Service_Where_Person_IsCorrect()
        {
            Person person = new Person("12345", "Alexey", 19);

            ValidationService<Person> personValidation = new ValidationService<Person>();
            ValidationResult result = personValidation.Validate(person);

            int expectedErrorsCount = 0;
            int actualErrorsCount = result.ValidationErrors.Count;

            Assert.AreEqual(expectedErrorsCount, actualErrorsCount);
        }

        [Test]
        public void Service_Where_Person_IsIncorrect()
        {
            Person person = new Person("12345", "Alexey", 19);
            person.PassportId = null;
            ValidationService<Person> personValidation = new ValidationService<Person>();
            ValidationResult result = personValidation.Validate(person);

            int expectedErrorsCount = 2;
            int actualErrorsCount = result.ValidationErrors.Count;

            Assert.AreEqual(expectedErrorsCount, actualErrorsCount);
        }

        [Test]
        public void CustomNotNullAttribute_Where_Person_IsCorrect()
        {
            Person person = new Person("12345", "Alexey", 19);
            CustomNotNullAttribute attribute = new CustomNotNullAttribute();

            bool actualValidationResult = attribute.IsValid(person.PassportId);

            Assert.IsTrue(actualValidationResult);
        }

        [Test]
        public void CustomNotNullAttribute_Where_Person_IsIncorrect()
        {
            Person person = new Person("12345", "Alexey", 19);
            person.PassportId = null;

            CustomNotNullAttribute attribute = new CustomNotNullAttribute();

            bool actualValidationResult = attribute.IsValid(person.PassportId);

            Assert.IsFalse(actualValidationResult);
        }

        [Test]
        public void CustomRangeAttribute_Where_Person_IsCorrect()
        {
            Person person = new Person("12345", "Alexey", 19);

            CustomRangeAttribute attribute = new CustomRangeAttribute(0,150);

            bool actualValidationResult = attribute.IsValid(person.Age);
            Assert.IsTrue(actualValidationResult);
        }

        [Test]
        public void CustomRangeAttribute_Where_Person_IsIncorrect()
        {
            Person person = new Person("12345", "Alexey", 19);
            person.Age = 160;

            CustomRangeAttribute attribute = new CustomRangeAttribute(0, 150);

            bool actualValidationResult = attribute.IsValid(person.Age);
            Assert.IsFalse(actualValidationResult);
        }

        [Test]
        public void CustomStringLengthAttribute_Where_Person_IsCorrect()
        {
            Person person = new Person("12345", "Alexey", 19);

            CustomStringLengthAttribute attribute = new CustomStringLengthAttribute(5,10);

            bool actualValidationResult = attribute.IsValid(person.Name);
            
            Assert.IsTrue(actualValidationResult);
        }

        [Test]
        public void CustomStringLengthAttribute_Where_Person_IsIncorrect()
        {
            Person person = new Person("12345", "Alexey", 19);
            person.Name = "a";

            CustomStringLengthAttribute attribute = new CustomStringLengthAttribute(5, 10);

            bool actualValidationResult = attribute.IsValid(person.Name);

            Assert.IsFalse(actualValidationResult);
        }

        [Test]
        public void Service_Where_Person_IsNull()
            => Assert.Throws<ArgumentNullException>(() => new ValidationService<Person>().Validate(null));
    }
}
