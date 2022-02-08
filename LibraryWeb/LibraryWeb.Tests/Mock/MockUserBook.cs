using Bogus;
using LibraryWeb.Models;

namespace LibraryWeb.Tests
{
    public class MockUserBook : BaseMockModel<UserBook>
    {
        public override void SetupConfig()
        {
            Faker.RuleFor(a => a.Id, f => f.IndexFaker);

            Faker.RuleFor(a => a.BookId, f => f.IndexFaker);

            Faker.RuleFor(a => a.UserId, f => f.Random.Guid().ToString());
        }
    }
}
