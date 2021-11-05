using System;
using System.IO;
using System.Linq;

namespace Library
{
    class Program
    {
        static void AddBooks()
        {
            var lines = File.ReadAllLines(@".\books.txt");
            Random random = new Random();
            int tempBook;
            int tempMagazine;
            string[] papersMetadata;
            for (uint i = 0; i < 10; i++)
            {
                do
                {
                    tempBook = random.Next(0, lines.Length - 1);
                    tempMagazine = random.Next(0, lines.Length - 1);
                } while (lines[tempBook].StartsWith("BOOK") && lines[tempMagazine].StartsWith("MAGAZINE"));

                papersMetadata = lines[tempBook].Split('|');
                Book temp1 = new Book(Convert.ToUInt32(papersMetadata[2], 16), papersMetadata[1], Convert.ToUInt32(papersMetadata[2], 16), 0);
                Library.Storage.Add(temp1);

                papersMetadata = lines[tempMagazine].Split('|');
                Magazine temp2 = new Magazine(0, Convert.ToUInt32(papersMetadata[2], 16), papersMetadata[1], Convert.ToUInt32(papersMetadata[2], 16), 0);
                Library.Storage.Add(temp2);
            }
        }

        static void AddPeople()
        {
            var lines = File.ReadAllLines(@".\female.txt");
            Random rand = new Random();
            int empPercent = rand.Next(9, 15);
            for (int i = 0; i < 30; i++)
            {
                if (i < empPercent)
                {
                    Employers temp = new Employers(lines[rand.Next(0, lines.Length - 1)], (uint)rand.Next(14, 80), "typicalGender");
                    Library.Members.Add(temp);
                }
                else
                {
                    Visitors temp = new Visitors(lines[rand.Next(0, lines.Length - 1)], (uint)rand.Next(14, 80), "typicalGender");
                    Library.Members.Add(temp);
                }
            }
        }

        static AbstractPeople[] TakeBooks()
        {
            //Random rand = new Random();
            int temp = -1;
            AbstractPeople[] takenBooked = new AbstractPeople[5];
            for (int i = 15; i < 20; i++)
            {
                temp++;
                takenBooked[temp] = Library.Members[i];
                for (int k = 0; k < 4; k++)
                    Library.Take(takenBooked[temp], Library.Storage.FirstOrDefault());
            }
            return takenBooked;
        }

        static void ReturnBooks(AbstractPeople[] debtPeople)
        {
            for (int i = 0; i < debtPeople.Length; i++)
            {
                Console.WriteLine(debtPeople[i].TakenBooks.Count);
                Console.WriteLine("Name " + debtPeople[i].Name);
                for (int j = 0; j < 4; j++)
                {
                    debtPeople[i].Return(debtPeople[i].TakenBooks.FirstOrDefault());
                }
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Library start count " + Library.GetNonTakenBooks());
            AddBooks();
            Console.WriteLine("Library after adding books and magazines = " + Library.GetNonTakenBooks());
            AddPeople();
            AbstractPeople[] peopleWithBooks = TakeBooks();
            Console.WriteLine("Library after taking the books " + Library.GetNonTakenBooks());
            
            Console.WriteLine();

            ReturnBooks(peopleWithBooks);
            Console.WriteLine("Returned books for first time " + Library.GetNonTakenBooks());

            for (int i = 0; i < 5; i++)
            {
                peopleWithBooks = TakeBooks();
                ReturnBooks(peopleWithBooks);
            }

            Console.WriteLine("Books after all iterations " + Library.GetNonTakenBooks());
        }
    }
}
