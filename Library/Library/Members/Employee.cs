using Library.Materials;
using Library.Enums;

namespace Library.Members
{
    public class Employee : AbstractMember
    {
        public Employee(string name, byte age, Gender gender, string role) : base(name, age, gender, role) { }

        public Book CopyBook(Book book)
        {
            return (Book)book.Clone();
        }
    }
}
