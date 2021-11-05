using System;

namespace Library
{
    public class Book : ICloneable
    {
        public readonly string Id;
        public readonly string Name;
        public readonly uint Year;
        public readonly uint PageCount;
        public int WearLevel { get; set; }

        public Book() { }

        public Book(uint year, string name, uint pageCount, int wearLevel)
        {
            Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            if (year >= 800 && year <= DateTime.Now.Year)
                Year = year;
            else
                Year = (uint)DateTime.Now.Year;

            Name = name;

            PageCount = pageCount;

            if (wearLevel >= 0 && wearLevel <= 5)
                WearLevel = wearLevel;
            else
                WearLevel = 0;
        }

        public void IncreaseWearLevel()
        {
            if (WearLevel < 5)
                WearLevel++;
            else
                Console.WriteLine("WearLevel Override for " + Name);
        }

        public object Clone()
        {
            return new Book(Year, Name + "-1.0", PageCount, WearLevel);
        }
    }

    public class Magazine : Book
    {
        public readonly int Number;

        public Magazine(int number, uint year, string name, uint pageCount, int wearLevel) : base(year, name, pageCount, wearLevel)
        {
            if (number < 0)
                Number = 0;

            else if (number > 12)
                Number = 12;

            else
                Number = number;

        }

        public static Magazine operator +(Magazine a, Magazine b)
        {
            return new Magazine(
                (a.Number + b.Number) / 2,
                a.Year,
                $"{a.Name}, {b.Name}",
                (a.PageCount + b.PageCount) / 2,
                a.WearLevel + b.WearLevel);
        }
    }
}
