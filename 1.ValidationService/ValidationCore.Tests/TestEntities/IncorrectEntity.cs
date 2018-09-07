using ValidationCOre.Attribute;

namespace ValidationCore.Tests.TestEntities
{
    public class IncorrectEntity
    {
        [CustomStringLength(10, MinLength = 0)]
        public int IncorrectInt { get; set; }

        [IntRange(0,10)]
        public string IncorrectString { get; set; }

        [CustomNotNull]
        public int? NullableInt { get; set; }

        public IncorrectEntity(int intProp, string strProp, int? nullInt)
        {
            IncorrectInt = intProp;
            IncorrectString = strProp;
            NullableInt = nullInt;
        }
    }
}
