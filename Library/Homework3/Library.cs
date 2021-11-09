using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework3;

namespace Homework3
{
    static class Library
    {
        public static List<Paper> Storage = new List<Paper>();
        public static List<People> Members = new List<People>();
        public static int GetNonTakenBooks() => Storage.Count;

        public static List<People> LibraryMembers
        {
            get
            {
                var array = Members.ToArray();
                Array.Sort(array, (a, b) => string.Compare(a.Name, b.Name));
                return array.ToList();
            }
        }

        public static void Take(People people, Paper book)
        {
            if (people.GetType() == typeof(Employers))
                throw new YouAreNotVisitorException();

            Library.Storage.Remove(book);
            book.IncreaseWearLevel();
            people.TakenBooks.Add(book);
        }

        public static Book Copy(People people, Paper book)
        {
            if (people.GetType() == typeof(Visitors))
                throw new YouAreNotVisitorException();

            return new Book(book.Year, book.Name + "-1.0", book.PageCount, book.WearLevel);
        }

        public static Paper FindBook(string bookName)
        {
            foreach (var item in Storage)
            {
                if (item.Name == bookName)
                    return item;
            }
            Console.WriteLine("Book with name " + bookName + " not found!");
            return null;
        }

        public static People FindPeople(string peopleName)
        {
            foreach (var item in Members)
            {
                if (item.Name == peopleName)
                    return item;
            }
            Console.WriteLine("People with name " + peopleName + " not found!");
            return null;
        }
    }
}
