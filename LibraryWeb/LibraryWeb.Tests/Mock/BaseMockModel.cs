using Bogus;
using System.Linq;

namespace LibraryWeb.Tests
{
    public abstract class BaseMockModel<T> : IMockConfiguration
       where T : class
    {
        protected BaseMockModel()
        {
            SetupConfig();
        }

        protected Faker<T> Faker { get; } = new Faker<T>();

        public T Instance => Faker.Generate();

        public abstract void SetupConfig();

        public T GetInstance(params string[] rules)
        {
            var rulesList = rules.ToList();

            if (!rulesList.Contains("default"))
                rulesList.Insert(0, "default");

            var ruleSet = string.Join(",", rulesList);
            return Faker.Generate(ruleSet);
        }
    }
}
