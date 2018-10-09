using System;
using DIResolver;
using Ninject;
using NUnit.Framework;
using Validation.Attributes;
using Validation.Interfaces;
using Validation.Service;
using Validation.Service.Helpers;
using Moq;
using Loggers;

namespace Validation.Tests
{
    [TestFixture]
    public class ValidationServiceTests
    {
        private static readonly IKernel _kernel = new StandardKernel();
        private readonly IValidationService<Person> personValidation;

        public ValidationServiceTests()
        {
            _kernel.ResolveDependencies();
            personValidation = _kernel.Get<IValidationService<Person>>();
        }

        [TestCase("12345", "Alexey", 19)]
        public void Service_Where_Person_Is_Correct(string passportId, string name, int age)
        {
            Person person = new Person(passportId, name, age);
            ValidationResult result = personValidation.Validate(person);

            Assert.IsNull(result.ValidationErrors);
        }
          
        [TestCase("12345", "Alexey", 160, 1)]
        [TestCase(null, "Alexey", 19, 2)]
        [TestCase(null, "Alexey", 160, 3)]
        public void Service_Where_Person_Is_Incorrect(string passportId, string name, int age, int expectedErrorsCount)
        {
            Person person = new Person(passportId, name, age);
            ValidationResult result = personValidation.Validate(person);

            int actualErrorsCount = result.ValidationErrors.Length;

            Assert.AreEqual(expectedErrorsCount, actualErrorsCount);
        }

        [TestCase("12345", "Alexey", 19, true)]
        [TestCase(null, "Alexey", 19, false)]
        public void CustomNotNullAttribute_With_Person(string passportId, string name, int age, bool expectedResult)
        {
            Person person = new Person(passportId, name, age);
            CustomNotNullAttribute attribute = new CustomNotNullAttribute();

            bool actualValidationResult = attribute.IsValid(person.PassportId);

            Assert.AreEqual(actualValidationResult, expectedResult);
        }

        [TestCase("12345", "Alexey", 19, true)]
        [TestCase("12345", "Alexey", 160, false)]
        public void CustomRangeAttribute_With_Person(string passportId, string name, int age, bool expectedResult)
        {
            Person person = new Person(passportId, name, age);
            CustomRangeAttribute attribute = new CustomRangeAttribute(0, 150);

            bool actualValidationResult = attribute.IsValid(person.Age);

            Assert.AreEqual(actualValidationResult, expectedResult);
        }

        [TestCase("12345", "Alexey", 19, true)]
        [TestCase("12345", "Al", 19, false)]
        public void CustomStringLengthAttribute_With_Person(string passportId, string name, int age, bool expectedResult)
        {
            Person person = new Person(passportId, name, age);

            CustomStringLengthAttribute attribute = new CustomStringLengthAttribute(5,10);

            bool actualValidationResult = attribute.IsValid(person.Name);

            Assert.AreEqual(actualValidationResult, expectedResult);
        }

        [Test]
        public void Service_Where_Logger_Works()
        {
            Person person = new Person("1234", "Alexey", 170);

            var loggerMock = new Mock<ILogger>();  
            ValidationService<Person> service = new ValidationService<Person>(loggerMock.Object);

            loggerMock.Setup(action => action.Warn(It.IsAny<string>()));
            service.Validate(person);

            loggerMock.Verify(a => a.Warn(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Service_Where_Person_IsNull()
            => Assert.Throws<ArgumentNullException>(() => personValidation.Validate(null));

        [Test]
        public void CustomNotRangeAttribute_Where_Minimum_Is_Bigger_Than_Maximum()
            => Assert.Throws<ArgumentException>(() => new CustomRangeAttribute(10, 2));

        [Test]
        public void CustomStringLengthAttribute_Where_Minimum_Is_Bigger_Than_Maximum()
            => Assert.Throws<ArgumentException>(() => new CustomStringLengthAttribute(10, 2));

        [Test]
        public void CustomStringLengthAttribute_Where_Minimum_Is_Less_Than_0()
            => Assert.Throws<ArgumentOutOfRangeException>(() => new CustomStringLengthAttribute(-1, 2));

        [Test]
        public void CustomStringLengthAttribute_Where_Maximum_Is_Less_Than_0()
            => Assert.Throws<ArgumentOutOfRangeException>(() => new CustomStringLengthAttribute(1, -2));
    }
}