using Library.Persons;
using System;
using System.Linq;

namespace Library.Exceptions
{
    public class LibraryException : Exception
    {
        public LibraryException(AbstractPerson abstractPerson)
        { 
            Console.Write($"{abstractPerson.Name} is " +
                $"{abstractPerson.GetType().ToString().Split('.').Last()}"); 
        }
    }
}
