using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Controllers;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using Xunit;

namespace LibraryApp.Tests
{   

    public static class DbContextMock
    {     

        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }
    }



    //Not working for now
    public class HomeControllerTests
    {
        private static readonly string[] fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

        [Fact]
        public void Test1()
        {
            var orderIds = 0;
            var testBooks = new Faker<Book>()
            .RuleFor(o => o.Id, f => orderIds++)
            .RuleFor(o => o.Name, f => f.PickRandom(fruit));


            var mock = new Mock<IBookRepository>();
            mock.Setup(repo => repo.GetBooks()).Returns(GetTestBooks());
            var controller = new HomeController(null, null, mock.Object);

            var result = controller.Index() as ViewResult;


            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BookViewModel>(viewResult.Model);
            Assert.Equal(DbContextMock.GetQueryableMockDbSet<Book>(testBooks).Count(), model.Book.ToList().Count());
        }
        private List<Book> GetTestBooks()
        {
            var books = new List<Book>
            {
                new Book { Id=1, Name="test1"},
                new Book { Id=2, Name="test2"},
                new Book { Id=3, Name="test3"},
                new Book { Id=4, Name="test4"}
            };
            return books;
        }

    }
}
