using Bogus;
using LibraryWeb.Controllers;
using LibraryWeb.Services;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Xunit;

namespace LibraryWeb.Tests
{
    public class TestHomeController
    {       
        [Fact]
        public void TestIndex_ShouldGetView()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(UserBooksViewModel), ((ViewResult)result).Model.GetType());
        }

        private IBookService GetBookService()
        {
            var book = Mock.Of<IBookService>();

            var data = new Faker()
                .Make(3, () => MockFactory.CreateObject<MockBook>().GetInstance("default"))
                .ToList();

            Moq.Mock.Get(book)
                .Setup(a =>
                    a.GetBooks())
                .Returns(data);

            return book;
        }

        private IUserBookService GetUserBookService()
        {
            var userBook = Mock.Of<IUserBookService>();

            var data = new Faker()
                .Make(1, () => MockFactory.CreateObject<MockUserBook>().GetInstance("default"))
                .ToArray();

            Moq.Mock.Get(userBook)
                .Setup(a =>
                    a.UserBooks)
                .Returns(data);

            return userBook;
        }

        private HomeController GetController()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            ILogger<HomeController> logger = loggerMock.Object;

            return new HomeController(logger, GetBookService(), GetUserBookService())
                .WithIdentity();
        }     
    }
}
