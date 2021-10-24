using Library.Exceptions;
using Library.Persons;
using Library.Readings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Library
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Books at the start={Library.GetNonTakenBooks()}");

            // Generate 10 random books and 10 magazines
            AddReadings(10, 10);
            Console.WriteLine($"Books after adding={Library.GetNonTakenBooks()}");

            // Generate 30 random library members (10-50% are employees)
            AddPeople(30);

            // Display sorted library members
            DisplayMembers();            

            for (var i = 0; i < 10; i++)
            {
                // Take books by random members
                TakeBooks();

                // Return books back to library Storage
                ReturnBooks();
            }

            // Try to copy a book for 5 randon members
            CopyBooks();

            // Create magazine supscription
            CreateSubscription(3);                 
        }

        private static void AddReadings(ushort booksAmount, ushort magazinesAmount)
        {
            Console.WriteLine($"\nAdd some readings...\n");

            var lines = File.ReadAllLines(@".\Resources\books.txt");

            var books = lines.Where(i => i.Split('|')[0].Equals("book", StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => Guid.NewGuid()).Take(booksAmount).ToArray();

            var magazines = lines.Where(i => i.Split('|')[0].Equals("magazine", StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => Guid.NewGuid()).Take(magazinesAmount).ToArray();

            foreach (var b in books)
                Library.Storage.Add(new Book(b.Split('|')[1], GetValidatedYear(b.Split('|')[2]),
                    Convert.ToUInt16(b.Split('|')[3]), Convert.ToByte(GetRandomNumber(0, 5))));

            foreach (var m in magazines)
                Library.Storage.Add(new Magazine(m.Split('|')[1], GetValidatedYear(m.Split('|')[2]),
                    Convert.ToUInt16(m.Split('|')[3]), Convert.ToByte(GetRandomNumber(0, 5)),
                    Convert.ToByte(GetRandomNumber(1, 12))));
        }

        private static void AddPeople(ushort amount)
        {
            Console.WriteLine($"\nAdd library members...");

            var men = File.ReadAllLines(@".\Resources\male.txt");
            var women = File.ReadAllLines(@".\Resources\female.txt");
            var employeeCount = Math.Round((float)amount * GetRandomNumber(10, 50) / 100);

            for (var i = 0; i < amount; i++)
            {
                var name = string.Empty;
                var gender = new Enums.Gender();

                if (GetRandomNumber(0, 1) == 0)
                {
                    name = men[GetRandomNumber(0, men.Length - 1)];
                    gender = Enums.Gender.Male;
                }
                else
                {
                    name = women[GetRandomNumber(0, women.Length - 1)];
                    gender = Enums.Gender.Female;
                }

                if (i < employeeCount)
                    Library.Members.Add(new Employee(name, Convert.ToByte(GetRandomNumber(10, 80)), gender));
                else
                    Library.Members.Add(new Visitor(name, Convert.ToByte(GetRandomNumber(10, 80)), gender));
            }
        }

        private static void TakeBooks()
        {
            Console.WriteLine($"\nTake books by random members:");
            var users = 0;
            foreach (var p in Library.Members.OrderBy(x => Guid.NewGuid()))
            {
                try
                {
                    Library.Storage.Where(i => i.GetType() == typeof(Book))
                        .OrderBy(x => Guid.NewGuid()).Take(2).ToList()
                        .ForEach(b => Library.Take(((Book)b).Name, p));

                    Library.Storage.Where(i => i.GetType() == typeof(Magazine))
                        .OrderBy(x => Guid.NewGuid()).Take(2).ToList()
                        .ForEach(b => Library.Take(((Book)b).Id, p));
                }
                catch (YouAreNotVisitorException Ex)
                {                    
                    continue;
                }

                Console.WriteLine($"Books at some moment of time={Library.GetNonTakenBooks()}");
                users++;

                if (users == 5)
                    break;
            }
        }

        private static void ReturnBooks()
        {            
            Console.WriteLine($"\nReturn books...");

            foreach (Visitor p in Library.Members.Where(i => i.GetType() == typeof(Visitor)
                && ((Visitor)i).TakenBooks.Count > 0))
            {
                p.OnUsedBookReturn -= DisplayMessage;
                p.OnUsedBookReturn += DisplayMessage;
                foreach (var b in p.TakenBooks.ToList())
                    p.Return(b);
            }
        }

        private static void CopyBooks()
        {
            Console.WriteLine($"\nTry to copy a book for 5 random members...\n");

            var people = Library.Members.OrderBy(x => Guid.NewGuid()).Take(5).ToArray();
            var copiedBooks = new List<Book>();

            foreach (var p in people)
            {
                if (p.GetType() == typeof(Visitor))
                    try
                    {
                        throw new YouAreNotEmployeeException((Visitor)p);
                    }
                    catch
                    {
                        continue;
                    }

                var randomBook = (Book)Library.Storage[GetRandomNumber(0, Library.GetNonTakenBooks())];
                copiedBooks.Add(((Employee)p).CopyBook(randomBook));
            }

            foreach (var cb in copiedBooks)
                Console.WriteLine($"CopiedBook: \"{cb.Name}\"|{cb.Year}" +
                    $"|{cb.Pages}|{cb.Wearout}");
        }

        private static void CreateSubscription(byte amount)
        {
            Console.WriteLine("\nCreate magazine subscription from:");

            var randomMagazines = Library.Storage.Where(i => i.GetType() == typeof(Magazine))
                .OrderBy(x => Guid.NewGuid()).Take(amount).Cast<Magazine>().ToList();

            if (amount > randomMagazines.Count)
            {
                Console.WriteLine("\nMagazines amount for subscription is more than existed:");
                return;
            }

            var magazines = new Magazine();

            foreach (var rm in randomMagazines)
            {
                Console.WriteLine($"\"{rm.Name}\"|{rm.Year}|{rm.Pages}|{rm.Wearout}|{rm.Number}");
                if (magazines.Id == null)
                    magazines = rm;
                else
                    magazines += rm;
            }

            Console.WriteLine($"\nCreated subscription:\n\"{magazines.Name}\"" +
                $"|{magazines.Year}|{magazines.Pages}|{magazines.Wearout}|{magazines.Number}");
        }

        private static void DisplayMembers()
        {
            Console.WriteLine("\nSorted library members:");
            foreach (var m in Library.LibraryMembers)
                Console.WriteLine($"{m.Name}|{m.Age}|{m.Gender}");
        }

        private static ushort GetValidatedYear(string input)
        {
            var year = Convert.ToUInt16(input);
            if (year < 800 || year > DateTime.Now.Year)
                return Convert.ToUInt16(GetRandomNumber(800, DateTime.Now.Year));

            return year;
        }

        private static int GetRandomNumber(int from, int to)
        {
            var r = new Random();
            return r.Next(from, to);
        }

        private static void DisplayMessage(Book book, Visitor visitor)
        {
            Console.WriteLine($"\"{book.Name}\" has been returned by {visitor.Name} in condition {book.Wearout}");
        }
    }
}
