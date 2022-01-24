using LibraryWeb.Controllers;
using LibraryWeb.Data;
using LibraryWeb.Models;
using LibraryWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LibraryWeb.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var mock = new Mock<ApplicationDbContext>();
            var dbContextMock = DbContextMock.GetQueryableMockDbSet<UserBook>(_testUserBooks);

            mock.Setup(context => context.UserBook)
                .Returns(dbContextMock);

            var controller = new HomeController(null, mock.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UserBooksViewModel>(viewResult.Model);
            Assert.Equal(DbContextMock.GetQueryableMockDbSet<UserBook>(_testUserBooks).Count(), model.Book.Count());
        }

        private static readonly List<UserBook> _testUserBooks = new List<UserBook>
        {
            new UserBook { Id=1, UserId=Guid.NewGuid().ToString(), BookId=3},
            new UserBook { Id=2, UserId=Guid.NewGuid().ToString(), BookId=5},
            new UserBook { Id=3, UserId=Guid.NewGuid().ToString(), BookId=7},
            new UserBook { Id=4, UserId=Guid.NewGuid().ToString(), BookId=9},
            new UserBook { Id=5, UserId=Guid.NewGuid().ToString(), BookId=10}
        };
    }
}
