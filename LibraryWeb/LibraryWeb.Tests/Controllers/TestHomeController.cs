using LibraryWeb.Controllers;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LibraryWeb.Tests.Controllers
{
    public class TestHomeController : BaseTestController<HomeController>
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

        protected override HomeController GetController()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            ILogger<HomeController> logger = loggerMock.Object;

            return new HomeController(logger, GetBookService(), GetUserBookService())
                .WithIdentity();
        }
    }
}
