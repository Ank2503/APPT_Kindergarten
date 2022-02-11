using LibraryWeb.Controllers;
using LibraryWeb.Models;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace LibraryWeb.Tests.Controllers
{
    public class TestBooksController : BaseTestController<BooksController>
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
            Assert.Equal(typeof(BooksViewModel), ((ViewResult)result).Model.GetType());
        }

        [Fact]
        public void TestAddBook_ShouldGetView()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = controller.AddBook() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Create", result.ViewName);
        }

        [Fact]
        public async Task TestAddBook_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = GetController();
            var book = MockFactory.CreateRandom<MockBook>().GetInstance();

            // Act
            var result = (RedirectToActionResult) await controller.AddBook(book);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact(Skip = "No test act")]
        public async void TestAddBook_ShouldUpdateEntity()
        {
            // Arrange
            var controller = GetController();

            // Act

            // Assert 
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task TestDetails_ShouldReturnNotFound()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(Skip = "Can't set an instance of the book")]
        public async Task TestDetails_ShouldGetView()
        {
            // Arrange
            var bookId = new Bogus.Faker().Random.Int(1, 100);
            var controller = GetController(bookId);

            // Act
            var result = (ViewResult) await controller.Details(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Details", result.ViewName);
            Assert.Equal(typeof(Book), ((ViewResult)result).Model.GetType());
        }

        [Fact]
        public async Task TestEdit_ShouldReturnNotFound()
        {
            // Arrange
            var controller = GetController();

            // Act
            var intResult = await controller.Edit((int?)null);
            var bookResult = await controller.Edit((Book)null);

            // Assert
            Assert.IsType<NotFoundResult>(intResult);
            Assert.IsType<NotFoundResult>(bookResult);
        }

        [Fact(Skip = "Can't set an instance of the book")]
        public async Task TestEdit_ShouldGetView()
        {
            // Arrange
            var bookId = new Bogus.Faker().Random.Int(1, 100);
            var controller = GetController(bookId);

            // Act
            var result = (ViewResult)await controller.Edit(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Edit", result.ViewName);
            Assert.Equal(typeof(Book), ((ViewResult)result).Model.GetType());
        }

        [Fact]
        public async Task TestEditBook_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = GetController();
            var book = MockFactory.CreateRandom<MockBook>().GetInstance();

            // Act
            var result = (RedirectToActionResult)await controller.Edit(book);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact(Skip = "No test act")]
        public async void TestEditBook_ShouldUpdateEntity()
        {
            // Arrange
            var controller = GetController();

            // Act

            // Assert 
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task TestDelete_ShouldReturnNotFound()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = await controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task TestDelete_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = GetController();
            var bookId = MockFactory.CreateRandom<MockBook>().GetInstance().Id;

            // Act
            var result = (RedirectToActionResult)await controller.Delete(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact(Skip = "No test act")]
        public async void TestDelete_ShouldUpdateEntity()
        {
            // Arrange
            var controller = GetController();

            // Act

            // Assert 
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task TestTake_ShouldReturnNotFound()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = await controller.Take(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task TestTake_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = GetController();
            var bookId = MockFactory.CreateRandom<MockBook>().GetInstance().Id;

            // Act
            var result = (RedirectToActionResult)await controller.Take(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact(Skip = "No test act")]
        public async void TestTake_ShouldUpdateEntity()
        {
            // Arrange
            var controller = GetController();

            // Act

            // Assert 
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task TestReturn_ShouldReturnNotFound()
        {
            // Arrange
            var controller = GetController();

            // Act
            var result = await controller.Return(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(Skip = "No test act")]
        public async Task TestReturn_ShouldRedirectToReferer()
        {
            // Arrange
            var controller = GetController();
            var bookId = MockFactory.CreateRandom<MockBook>().GetInstance().Id;

            // Act
            var result = (RedirectResult)await controller.Return(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.Url);
        }

        [Fact(Skip = "No test act")]
        public async void TestReturn_ShouldUpdateEntity()
        {
            // Arrange
            var controller = GetController();

            // Act

            // Assert 
            Assert.NotNull(controller);
        }

        protected override BooksController GetController()
        {
            return new BooksController(GetBookService(), GetUserBookService())
                .WithIdentity();
        }

        private BooksController GetController(int bookId)
        {
            return new BooksController(GetBookService(bookId), GetUserBookService())
                .WithIdentity();
        }
    }
}
