using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public interface IBookRepository
    {
        Book Get(int? id);

        List<Book> GetBooks();

        List<UserBook> GetUserBooks();
    }
}
