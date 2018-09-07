using ValidationCOre.Attribute;

namespace ValidationCore.Tests
{
    public class Person
    {
        [CustomRequired]
        [CustomNotNull]
        public string PassportId { get; set; }

        [CustomRequired]
        [CustomStringLength(10, MinLength = 3)]
        public string Name { get; set; }

        [IntRange(0, 100)]
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
