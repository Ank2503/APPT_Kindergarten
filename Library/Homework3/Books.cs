using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    public class Paper { 
        string Id;
        public string Name { get; set; }
        public uint Year { get; set; }
        public uint PageCount { get; set; }
        public int WearLevel { get; set; }
        public delegate void WearoutHandler(Paper book);
        public event WearoutHandler OnUsedBookReturn;

        public Paper(uint year, string name, uint pageCount, int wearLevel)
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
            if(WearLevel > 3)
                OnUsedBookReturn?.Invoke(this);
        }
    
    }

    public class Book : Paper
    {
        public Book(uint year, string name, uint pageCount, int wearLevel) : base(year, name, pageCount, wearLevel) { }
       
    }

    public class Magazine : Book
    {
        public int Number;

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
