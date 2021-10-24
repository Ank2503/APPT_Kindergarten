using Library.Enums;
using Library.Readings;
using System.Collections.Generic;

namespace Library.Persons
{
    public class Visitor : AbstractPerson
    {
        public List<Book> TakenBooks = new List<Book>();
        public delegate void WearoutHandler(Book book, Visitor visitor);
        public event WearoutHandler OnUsedBookReturn;

        public Visitor(string name, byte age, Gender gender) : base(name, age, gender) { }

        public void Return(Book book) 
        {
            this.TakenBooks.Remove(book);

            if (book.Wearout < 5)
                book.Wearout++;

            if (book.Wearout > 3)
                OnUsedBookReturn?.Invoke(book, this);

            Library.Storage.Add(book);            
        }
    }
}
