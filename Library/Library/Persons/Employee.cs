using Library.Enums;
using Library.Readings;

namespace Library.Persons
{
    public class Employee : AbstractPerson
    {
        public Employee(string name, byte age, Gender gender) : base(name, age, gender) { }

        public Book CopyBook(Book book) 
        {
            return (Book)book.Clone();
        }
    }
}
