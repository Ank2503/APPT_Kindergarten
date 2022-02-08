using LibraryWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryWeb.Services
{
    public interface IBookService
    {
        public List<Book> GetBooks();

        public Book GetBook(int? id);

        public Task AddBook(Book book);

        public Task UpdateBook(Book book);

        public Task DeleteBook(Book book);
    }
}
