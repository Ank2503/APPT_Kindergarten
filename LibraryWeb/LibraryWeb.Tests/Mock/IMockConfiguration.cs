using Bogus;
using Microsoft.Extensions.Options;
using Moq;

namespace LibraryWeb.Tests
{
    public interface IMockConfiguration
    {
        /// <summary>
        /// Setup instance properties with random data
        /// </summary>
        void SetupConfig();
    }

    public abstract class BaseMockConfiguration<T> : IMockConfiguration where T : class
    {
        protected Faker<T> Faker { get; } = new Faker<T>();

        public T Instance => Faker.Generate();

        public abstract void SetupConfig();
    }

    public class MockConfigOptionsFactory<T> where T : class, new()
    {
        private MockConfigOptionsFactory(T value)
        {
            var mock = new Mock<IOptions<T>>();

            mock.Setup(x => x.Value).Returns(() => value);

            Options = mock.Object;
        }

        private IOptions<T> Options { get; }

        public static IOptions<T> GetOptions(T value)
        {
            var mockConfigFactory = new MockConfigOptionsFactory<T>(value);

            return mockConfigFactory.Options;
        }
    }
}
