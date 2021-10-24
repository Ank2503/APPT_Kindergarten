using Library.Persons;
using Library.Readings;
using Library.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    static class Library
    {
        public static List<object> Storage = new List<object>();
        public static List<AbstractPerson> Members = new List<AbstractPerson>();
        public static List<AbstractPerson> LibraryMembers
        {
            get 
            {
                var sortedMembers = Members.ToArray();
                Array.Sort(sortedMembers, (x, y) => String.Compare(x.Name, y.Name));
                return sortedMembers.ToList();
            }
        }

        public static ushort GetNonTakenBooks() 
        { 
            return (ushort)Storage.Where(i => i.GetType() == typeof(Book)).ToArray().Length; 
        }

        public static void Take(string book, AbstractPerson person)
        {
            if (person.GetType() == typeof(Employee))                
                    throw new YouAreNotVisitorException((Employee)person);

            var takenBook = Storage.Where(i => ((Book)i).Name.Equals(book, StringComparison.OrdinalIgnoreCase) 
                || ((Book)i).Id.Equals(book, StringComparison.OrdinalIgnoreCase))
                .First();

            if (takenBook == null)
            {
                Console.WriteLine($"{book} doesn't exist in storage!");
            }
            else
            {
                ((Visitor)person).TakenBooks.Add((Book)takenBook);
                Storage.Remove(takenBook);               
            }
        }
     }
}
