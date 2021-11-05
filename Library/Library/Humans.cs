using System;
using System.Collections.Generic;

namespace Library
{
    public abstract class AbstractPeople : IComparable
    {
        public readonly string Name;
        public readonly uint Age;
        public readonly string Gender;
        public List<Book> TakenBooks = new List<Book>();


        public AbstractPeople(string name, uint age, string gender)
        {
            Name = name;
            Age = age;
            Gender = gender;
        }

        public int CompareTo(object obj)
        {
            if (obj is AbstractPeople p)
                return Name.CompareTo(p.Name);
            else
                throw new Exception("Can't compare person objects");
        }

        public void Return(Book book)
        {
            TakenBooks.Remove(book);
            Library.Storage.Add(book);
        }
    }

    public class Visitors : AbstractPeople
    {
        public Visitors(string name, uint age, string gender) : base(name, age, gender) { }
    }

    public class Employers : AbstractPeople
    {
        public Employers(string name, uint age, string gender) : base(name, age, gender) { }
    }
}
