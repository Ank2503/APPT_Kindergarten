using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class People
    {
        public string name;
        public uint age;
        public string gender;
        public List<Book> TakenBooks = new List<Book>();


        public People(string Name, uint Age, string Gender)
        {
            this.name = Name;
            this.age = Age;
            this.gender = Gender;
        }

        public void Return(Book book)
        {
            this.TakenBooks.Remove(book);
            Library.Storage.Add(book);
        }
    }

    public class Visitors : People
    {
        public Visitors(string Name, uint Age, string Gender) : base(Name, Age, Gender) { }
    }

    public class Employers : People
    {
        public Employers(string Name, uint Age, string Gender) : base(Name, Age, Gender) { }
    }
}
