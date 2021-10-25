using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Homework3
{   
    class Program
    {      
        static void addBooks()
        {
            var lines = File.ReadAllLines(@".\books.txt");
            Random rand = new Random();
            int tempBook = 0;
            int tempMagazine = 0;
            string[] papersMetadata = new string[4];
            for (uint i=0; i<10; i++)
            {
                do
                {
                    tempBook = rand.Next(0, lines.Length - 1);
                    tempMagazine = rand.Next(0, lines.Length - 1);
                } while (lines[tempBook].StartsWith("BOOK") && lines[tempMagazine].StartsWith("MAGAZINE"));

                papersMetadata = lines[tempBook].Split('|');
                Book temp1 = new Book(Convert.ToUInt32(papersMetadata[2], 16), papersMetadata[1], Convert.ToUInt32(papersMetadata[2], 16), 0);
                Library.Storage.Add(temp1);

                papersMetadata = lines[tempMagazine].Split('|');
                Magazine temp2 = new Magazine(0, Convert.ToUInt32(papersMetadata[2], 16), papersMetadata[1], Convert.ToUInt32(papersMetadata[2], 16), 0);
                Library.Storage.Add(temp2);
            }
        }

        static void addPeople()
        {
            var lines = File.ReadAllLines(@".\female.txt");
            Random rand = new Random();
            int empPercent = rand.Next(9, 15);
            for(int i=0; i<30; i++)
            {
                if(i<empPercent)
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

        static People[] takeBooks()
        {
            Random rand = new Random();
            int temp = -1;
            People[] takenBooked = new People[5];
            for (int i = 15; i < 20; i++)
            {
                temp++;
                takenBooked[temp] = Library.Members[i];
                for (int k=0; k < 4; k++)
                    Library.Take(takenBooked[temp], Library.Storage.FirstOrDefault());
            }           
            return takenBooked;
        }

        static void returnBooks(People[] debtPeople)
        {
            for (int i = 0; i < debtPeople.Length; i++)
            {
                Console.WriteLine(debtPeople[i].TakenBooks.Count);
                Console.WriteLine("Name" + debtPeople[i].name);
                for(int j = 0; j < 4; j++)
                {
                    debtPeople[i].Return(debtPeople[i].TakenBooks.FirstOrDefault());
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Library start count" + Library.GetNonTakenBooks());
            addBooks();
            Console.WriteLine("Library after adding books and magazines = " +Library.GetNonTakenBooks());
            addPeople();
            People[] peopleWithBooks = takeBooks();
            Console.WriteLine("Library after taking the books" + Library.GetNonTakenBooks());

            returnBooks(peopleWithBooks);
            Console.WriteLine("Returned books for first time" + Library.GetNonTakenBooks());

            for(int i=0;i<5;i++)
            {
                peopleWithBooks = takeBooks();
                returnBooks(peopleWithBooks);
            }

            Console.WriteLine("Books after all iterations" + Library.GetNonTakenBooks());
        }
    }
}
