using System;
using System.Collections.Generic;
using System.Linq;
using Library.Members;
using Library.Materials;
using Library.Exceptions;

namespace Library
{
    class Library
    {
        public static List<object> Storage = new();
        public static List<AbstractMember> Members = new();
        public static List<AbstractMember> LibraryMembers
        {
            get
            {
                var sortedMembers = Members.ToArray();
                Array.Sort(sortedMembers, (x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                return sortedMembers.ToList();
            }
        }

        public static ushort GetNonTakenBooks()
        {
            return (ushort)Storage.Where(i => i.GetType() == typeof(Book)).ToArray().Length;
        }

        public static ushort GetNonTakenMagazines()
        {
            return (ushort)Storage.Where(i => i.GetType() == typeof(Magazine)).ToArray().Length;
        }

        public static void Take(string book, AbstractMember member)
        {
            if (member.GetType() == typeof(Employee))
                throw new YouAreNotVisitorException((Employee)member);

            var takenBook = Storage.Where(i => ((Book)i).Name.Equals(book, StringComparison.OrdinalIgnoreCase)
                || ((Book)i).Id.Equals(book, StringComparison.OrdinalIgnoreCase))
                .First();

            if (takenBook == null)
            {
                Console.WriteLine($"{book} doesn't exist in storage!");
            }
            else
            {
                ((Visitor)member).TakenBooks.Add((Book)takenBook);
                Storage.Remove(takenBook);
            }
        }
    }
}
