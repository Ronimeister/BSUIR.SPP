using Validation.Attributes;

namespace Validation.Tests
{
    public class Person
    {
        [CustomNotNull]
        [CustomStringLength(3, 10)]
        public string PassportId { get; set; }

        [CustomStringLength(5, 20)]
        public string Name { get; set; }

        [CustomRange(0,150)]
        public int Age { get; set; }

        public Person(string passportId, string name, int age)
        {
            PassportId = passportId;
            Name = name;
            Age = age;
        }

        public override string ToString() => $"Person info:\n PassportId:{PassportId}\n Name: {Name}\n Age:{Age}";
    }
}
