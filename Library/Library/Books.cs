using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class Book
    {
        string id;
        public string name { get; set; }
        public uint year;
        public uint pageCount;
        public int wearLevel { get; set; }

        public Book(uint Year, string Name, uint PageCount, int WearLevel)
        {
            this.id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            if (Year >= 800 && Year <= DateTime.Now.Year)
                this.year = Year;
            else
                this.year = (uint)DateTime.Now.Year;

            this.name = Name;

            this.pageCount = PageCount;

            if (WearLevel >= 0 && WearLevel <= 5)
                this.wearLevel = WearLevel;
            else
                this.wearLevel = 0;
        }

        public void IncreaseWearLevel()
        {
            if (this.wearLevel < 5)
                this.wearLevel++;
            else
                Console.WriteLine("WearLevel Override for " + this.name);

        }

        public Book Copy()
        {
            return new Book(this.year, this.name + "-1.0", this.pageCount, this.wearLevel);
        }
    }

    public class Magazine : Book
    {
        public int number;

        public Magazine(int Number, uint Year, string Name, uint PageCount, int WearLevel) : base(Year, Name, PageCount, WearLevel)
        {
            if (Number < 0)
                this.number = 0;

            else if (Number > 12)
                this.number = 12;

            else
                this.number = Number;

        }

        public static Magazine operator +(Magazine a, Magazine b)
        {
            return new Magazine(
                (a.number + b.number) / 2,
                a.year,
                $"{a.name}, {b.name}",
                (a.pageCount + b.pageCount) / 2,
                a.wearLevel + b.wearLevel);
        }
    }
}
