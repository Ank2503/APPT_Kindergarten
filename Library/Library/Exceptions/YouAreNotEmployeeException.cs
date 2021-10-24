using Library.Persons;
using System;

namespace Library.Exceptions
{
    public class YouAreNotEmployeeException : LibraryException
    {
        public YouAreNotEmployeeException(Visitor visitor) : base(visitor)
        {
            Console.WriteLine(" (YouAreNotEmployeeException)");
        }
    }
}
