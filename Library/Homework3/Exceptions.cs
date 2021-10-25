using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
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
