using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    public class People
    {
        public string Name;
        public uint Age;
        public string Gender;
        public List<Paper> TakenBooks = new List<Paper>();


        public People(string name, uint age, string gender)
        {
            Name = name;
            Age = age;
            Gender = gender;
        }

        public void Return(Paper book)
        {
            this.TakenBooks.Remove(book);
            Library.Storage.Add(book);
        }
    }

    public class Visitors : People
    {
        public Visitors(string name, uint age, string gender) : base(name, age, gender) { }
      
    }

    public class Employers : People
    {       
        public Employers(string name, uint age, string gender) : base(name, age, gender) { }
    }
}
