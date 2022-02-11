using Bogus;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;

namespace LibraryWeb.Tests.Controllers
{
    public abstract class BaseTestController<T> where T : ControllerBase
    {
        protected abstract T GetController();

        protected IBookService GetBookService(int id = 0)
        {
            var book = Mock.Of<IBookService>();

            var data = new Faker()
                .Make(3, () => MockFactory.CreateObject<MockBook>().GetInstance("default"))
                .ToList();

            if (id != 0)
                data.First().Id = id;

            Moq.Mock.Get(book)
                .Setup(a =>
                    a.GetBooks())
                .Returns(data);

            return book;
        }

        protected IUserBookService GetUserBookService()
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
    }
}
