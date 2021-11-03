using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class LibraryException : Exception
    {
        public LibraryException()
        {
            Console.WriteLine("Error working with people");
        }
    }
    public class YouAreNotEmployeeException : LibraryException
    {
        public YouAreNotEmployeeException()
        {
            Console.WriteLine("Error working with visitor");
        }
    }

    public class YouAreNotVisitorException : LibraryException
    {
        public YouAreNotVisitorException()
        {
            Console.WriteLine("Error working with employee");
        }
    }
}
