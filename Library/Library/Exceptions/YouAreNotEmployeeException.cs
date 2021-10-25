using System;
using Library.Members;

namespace Library.Exceptions
{
    public class YouAreNotEmployeeException : BaseException
    {
        public YouAreNotEmployeeException(Visitor visitor) : base(visitor)
        {
            Console.WriteLine(" (YouAreNotEmployeeException)");
        }
    }
}
