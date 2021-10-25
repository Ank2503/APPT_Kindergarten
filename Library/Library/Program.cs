using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Library.Members;
using Library.Materials;
using Library.Exceptions;

namespace Library
{
    class Program
    {

        private static void Main(string[] args)
        {
            GetStorageContent();
            Console.WriteLine($"\nAdding materials to the storage.\n");
 
            AddMaterials(10, 10);
            GetStorageContent();

            AddMember(30);
            DisplayMembers();

            for (var i = 0; i < 10; i++)
            {
                TakeBooks();
                ReturnBooks();
            }

            CopyBooks();

            CreateSubscription(3);
        }

        private static void GetStorageContent()
        {
            Console.WriteLine($"There are {Library.GetNonTakenBooks()} books at the storage right now.");
            Console.WriteLine($"There are {Library.GetNonTakenMagazines()} magazines at the storage right now.");
        }

        private static void AddMaterials(ushort booksCount, ushort magazinesCount)
        {
            var content = File.ReadAllLines(@".\Resources\books.txt");

            var books = content.Where(i => i.Split('|')[0].Equals("BOOK"))
                .OrderBy(x => Guid.NewGuid()).Take(booksCount).ToArray();

            var magazines = content.Where(i => i.Split('|')[0].Equals("MAGAZINE"))
                .OrderBy(x => Guid.NewGuid()).Take(magazinesCount).ToArray();

            foreach (var b in books)
                Library.Storage.Add(new Book(b.Split('|')[1], GetValidatedYear(b.Split('|')[2]),
                    Convert.ToUInt16(b.Split('|')[3]), Convert.ToByte(GenerateRandomNumber(0, 5))));

            foreach (var m in magazines)
                Library.Storage.Add(new Magazine(m.Split('|')[1], GetValidatedYear(m.Split('|')[2]),
                    Convert.ToUInt16(m.Split('|')[3]), Convert.ToByte(GenerateRandomNumber(0, 5)),
                    Convert.ToByte(GenerateRandomNumber(1, 12))));
        }

        private static void AddMember(ushort amount)
        {
            var men = File.ReadAllLines(@".\Resources\male.txt");
            var female = File.ReadAllLines(@".\Resources\female.txt");

            var employeeCount = Math.Round((float)amount * GenerateRandomNumber(10, 50) / 100);

            for (var i = 0; i < amount; i++)
            {
                string name;
                Enums.Gender gender;
                int genderRandomizer = GenerateRandomNumber(0, 2);

                if (genderRandomizer == 0)
                {
                    name = men[GenerateRandomNumber(0, men.Length)];
                    gender = Enums.Gender.Male;
                }
                else
                {
                    name = female[GenerateRandomNumber(0, female.Length)];
                    gender = Enums.Gender.Female;
                }

                if (i < employeeCount)
                    Library.Members.Add(
                        new Employee(
                            name, 
                            Convert.ToByte(GenerateRandomNumber(10, 80)), 
                            gender, 
                            "Employee")
                        );
                else
                    Library.Members.Add(
                        new Visitor(
                            name, 
                            Convert.ToByte(GenerateRandomNumber(10, 80)), 
                            gender, 
                            "Visitor")
                        );
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
                        .ForEach(b => Library.Take(((Magazine)b).Name, p));
                }
                catch (YouAreNotVisitorException Ex)
                {
                    continue;
                }

                GetStorageContent();
                users++;

                if (users == 5)
                    break;
            }
        }

        private static void ReturnBooks()
        {
            Console.WriteLine($"\nReturned materials: ");

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
            Console.WriteLine($"\nTrying to copy a book for 5 random members.\n");

            var members = Library.Members.OrderBy(x => Guid.NewGuid()).Take(5).ToArray();
            var copiedBooks = new List<Book>();

            foreach (var p in members)
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

                var randomBook = (Book)Library.Storage[GenerateRandomNumber(0, Library.GetNonTakenBooks())];
                copiedBooks.Add(item: (p as Employee).CopyBook(randomBook));
            }

            foreach (var cb in copiedBooks)
                Console.WriteLine($"CopiedBook: \"{cb.Name}\"|{cb.Year}" +
                    $"|{cb.Pages}|{cb.Deterioration}");
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
                Console.WriteLine($"\"{rm.Name}\"|{rm.Year}|{rm.Pages}|{rm.Deterioration}|{rm.Number}");
                if (magazines.Id == null)
                    magazines = rm;
                else
                    magazines += rm;
            }

            Console.WriteLine($"\nCreated subscription:\n\"{magazines.Name}\"" +
                $"|{magazines.Year}|{magazines.Pages}|{magazines.Deterioration}|{magazines.Number}");
        }

        private static void DisplayMembers()
        {
            Console.WriteLine($"\nSorted library members:");
            foreach (var m in Library.LibraryMembers)
                Console.WriteLine($"{m.Name}|{m.Age}|{m.Gender}|{m.Role}");
        }

        private static ushort GetValidatedYear(string input)
        {
            var year = Convert.ToUInt16(input);
            if (year < 800 || year > DateTime.Now.Year)
                return Convert.ToUInt16(GenerateRandomNumber(800, DateTime.Now.Year));

            return year;
        }

        private static int GenerateRandomNumber(int minValue, int maxValue)
        {
            var r = new Random();
            return r.Next(minValue, maxValue);
        }

        private static void DisplayMessage(Book book, Visitor visitor)
        {
            Console.WriteLine($"\"{book.Name}\" has been returned by {visitor.Name} in condition {book.Deterioration}");
        }
    }
}
