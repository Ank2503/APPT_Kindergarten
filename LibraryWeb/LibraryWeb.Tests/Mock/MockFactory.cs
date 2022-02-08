using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using System;

namespace LibraryWeb.Tests
{
    public static class MockFactory
    {
        /// <summary>
        /// Use Moq.Mock.Of() for generating object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateObject<T>() where T : class
        {
            return Mock.Of<T>();
        }

        /// <summary>
        /// Used for creating a service that is set up by Moq.Mock
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateService<T>() where T : IMockService
        {
            return Activator.CreateInstance<T>();
        }
        /// <summary>
        /// Used for creating an object that is set up by Moq.Mock
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T MockCreateRandom<T>() where T : IMockService
        {
            return CreateService<T>();
        }

        /// <summary>
        /// Used for ceatinng an object that is set up by Faker
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static T CreateRandom<T>(params string[] rules) where T : class
        {
            var ruleSet = string.Join(",", rules);

            return new Faker<T>().Generate(ruleSet);
        }

        /// <summary>
        /// Used for ceatinng a configuration object that is set up by Faker
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateConfig<T>() where T : IMockConfiguration
        {
            var mockConfig = Activator.CreateInstance<T>();

            mockConfig.SetupConfig();

            return mockConfig;
        }

        /// <summary>
        /// Used for creating IOptions<T> object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Configuration value</param>
        /// <returns></returns>
        public static IOptions<T> GetOptions<T>(T value) where T : class, new()
        {
            var options = new Mock<IOptions<T>>();

            options.Setup(x => x.Value).Returns(() => value);

            return options.Object;
        }
    }
}
