using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Library
{
    static class Library
    {
        public static List<Book> Storage = new List<Book>();
        public static List<People> Members = new List<People>();
        public static int GetNonTakenBooks() => Storage.Count;

        public static List<People> LibraryMembers
        {
            get
            {
                var array = Members.ToArray();
                Array.Sort(array, (a, b) => string.Compare(a.name, b.name));
                return array.ToList();
            }
        }

        public static void Take(People people, Book book)
        {
            if (people.GetType() == typeof(Employers))
                throw new YouAreNotVisitorException();

            Library.Storage.Remove(book);
            book.IncreaseWearLevel();
            people.TakenBooks.Add(book);
        }

        public static Book FindBook(string bookName)
        {
            foreach (var item in Storage)
            {
                if (item.name == bookName)
                    return item;
            }
            Console.WriteLine("Book with name " + bookName + " not found!");
            return null;
        }

        public static People FindPeople(string peopleName)
        {
            foreach (var item in Members)
            {
                if (item.name == peopleName)
                    return item;
            }
            Console.WriteLine("People with name " + peopleName + " not found!");
            return null;
        }
    }
}
