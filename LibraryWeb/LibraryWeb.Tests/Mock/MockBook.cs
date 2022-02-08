using Bogus;
using LibraryWeb.Models;

namespace LibraryWeb.Tests
{
    public class MockBook : BaseMockModel<Book>
    {
        public override void SetupConfig()
        {
            Faker.RuleFor(a => a.Id, f => f.IndexFaker);

            Faker.RuleFor(a => a.Name, f => f.Lorem.Text());            
        }
    }
}
