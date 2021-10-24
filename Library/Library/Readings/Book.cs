using System;

namespace Library.Readings
{
    public class Book : ICloneable
    {
        public readonly string Id;
        public readonly string Name;
        public readonly ushort Year;
        public readonly ushort Pages;
        public byte Wearout;

        public Book() { }

        public Book(string name, ushort year, ushort pages, byte wearout)
        {
            Id = Guid.NewGuid().ToString("N");
            Name = name;
            Year = year;
            Pages = pages;
            Wearout = wearout;
        }

        public object Clone()
        {
            return new Book(this.Name + "-copy", this.Year, this.Pages, this.Wearout);
        }
    }
}
