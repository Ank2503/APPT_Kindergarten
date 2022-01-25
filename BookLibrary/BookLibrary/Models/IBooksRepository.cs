using System.Collections.Generic;

namespace BookLibrary.Models
{
    public interface IBooksRepository
    {
        List<Book> GetBooks();
    }
}
