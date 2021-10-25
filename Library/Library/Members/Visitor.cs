using System.Collections.Generic;
using Library.Materials;
using Library.Enums;

namespace Library.Members
{
    public class Visitor : AbstractMember
    {
        public List<Book> TakenBooks = new();
        public delegate void DeteriorationHandler(Book book, Visitor visitor);
        public event DeteriorationHandler OnUsedBookReturn;

        public Visitor(string name, byte age, Gender gender, string role) : base(name, age, gender, role) { }

        public void Return(Book book)
        {
            TakenBooks.Remove(book);

            if (book.Deterioration < 5)
                book.Deterioration++;

            if (book.Deterioration > 3)
                OnUsedBookReturn?.Invoke(book, this);

            Library.Storage.Add(book);
        }
    }
}
