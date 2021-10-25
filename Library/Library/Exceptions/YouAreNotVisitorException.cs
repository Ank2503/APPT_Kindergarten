using System;
using Library.Members;

namespace Library.Exceptions
{
    public class YouAreNotVisitorException : BaseException
    {
        public YouAreNotVisitorException(Employee employee) : base(employee)
        {
            Console.WriteLine(" (YouAreNotVisitorException)");
        }
    }
}
