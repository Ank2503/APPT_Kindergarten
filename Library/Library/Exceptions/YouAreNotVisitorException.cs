using Library.Persons;
using System;

namespace Library.Exceptions
{
    public class YouAreNotVisitorException : LibraryException
    {
        public YouAreNotVisitorException(Employee employee) : base(employee) 
        {
            Console.WriteLine(" (YouAreNotVisitorException)");
        }
    }
}
