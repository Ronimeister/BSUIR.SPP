using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ValidationCore.Tests.TestEntities;
using ValidationCOre.Service;
using Logger;

namespace ValidationCore.Tests
{
    [TestFixture]
    public class ValidationTests
    {
        private readonly Person person = new Person("123456", "Alexey", 19);
        private readonly IncorrectEntity incorEnt = new IncorrectEntity(10, "foobar", 19);

        private readonly ValidationService service = ValidationService.Instance(new NLogLogger());

        [Test]
        public void Person_TryValidateObject_CorrectObject_IsCorrect()
        {
            bool actualResult = service.TryValidateObject(person, new ValidationContext(person),
                new List<ValidationResult>(), true);

            Assert.IsTrue(actualResult);
        }

        [Test]
        public void Person_TryValidateObject_NotNull_IsCorrect()
        {
            person.PassportId = null;

            bool actualResult = service.TryValidateObject(person, new ValidationContext(person),
                new List<ValidationResult>(), true);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Person_TryValidateObject_Required_IsCorrect()
        {
            person.PassportId = null;

            bool actualResult = service.TryValidateObject(person, new ValidationContext(person),
                new List<ValidationResult>(), true);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Person_TryValidateObject_IntRange_IsCorrect()
        {
            person.Age = -1;

            bool actualResult = service.TryValidateObject(person, new ValidationContext(person),
                new List<ValidationResult>(), true);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void Person_TryValidateObject_StringLength_IsCorrect()
        {
            person.Name = "ab";

            bool actualResult = service.TryValidateObject(person, new ValidationContext(person),
                new List<ValidationResult>(), true);

            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IncorrectEntity_TryValidateObject_InvalidCastException()
            => Assert.Throws<InvalidCastException>(() => service.TryValidateObject(incorEnt,
                new ValidationContext(incorEnt),
                new List<ValidationResult>(), true));
    }
}
