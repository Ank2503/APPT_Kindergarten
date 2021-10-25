using System;
using System.Linq;
using Library.Members;

namespace Library.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(AbstractMember abstractMember)
        {
            Console.Write($"{abstractMember.Name} is " +
                $"{abstractMember.GetType().ToString().Split('.').Last()}");
        }
    }
}
