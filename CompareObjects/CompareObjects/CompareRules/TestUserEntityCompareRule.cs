using CompareObjects.Contracts;
using CompareObjects.TestModels;

namespace CompareObjects.CompareRules
{
    public class TestUserEntityCompareRule : ICompareRule<TestUserEntity>
    {
        public CompareSettings OnConfigure(CompareBuilder<TestUserEntity> builder)
        {
            builder.CaseSensitiveOn();
            builder.RecursiveOn();
            builder.IgnoreField(x => x.Id);
            builder.SetTextForProperty(x => x.Message, "[Name] has been changed");
            builder.SetDisplayNameForProperty(x=>x.Message, "Status");
            return builder.Build();
        }
    }
}
