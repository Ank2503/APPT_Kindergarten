using BookLibrary.Controllers;
using BookLibrary.Data;
using BookLibrary.Models;
using BookLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BookLibrary.Tests
{
    public class BooksControllerTests
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfBooks()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(databaseName: "aspnet-BookLibrary-FCB67E06-C60A-4312-AF9E-92B1E1F80993")
                  .Options;

            var context = new ApplicationDbContext(options);

            var mock = new Mock<IBooksRepository>();
            mock.Setup(repo => repo.GetBooks()).Returns(GetTestBooks());
            var controller = new BooksController(context, mock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BooksViewModel>(viewResult.Model);
        }
        private List<Book> GetTestBooks()
        {
            var books = new List<Book>
            {
                new Book { Id=1, Name="Phantoms in the Brain: Probing the Mysteries of the Human Mind – V.S. Ramachandran and Sandra Blakeslee"},
                new Book { Id=2, Name="The Language Instinct: How the Mind Creates Language – Steven Pinker"},
                new Book { Id=3, Name="C#. Basics"},
                new Book { Id=4, Name="Romeo and Juliet"}
            };
            return books;
        }
    }
}