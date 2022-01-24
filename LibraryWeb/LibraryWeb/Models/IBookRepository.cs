using System.Collections.Generic;

namespace LibraryWeb.Models
{
    public interface IBookRepository
    {
        public List<Book> GetBooks();
        public Book Get(int? id);
    }
}
