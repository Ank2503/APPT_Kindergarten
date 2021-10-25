using System;

namespace Library.Materials
{
    public class Book : ICloneable
    {
        public readonly string Id;
        public readonly string Name;
        public readonly ushort Year;
        public readonly ushort Pages;
        public byte Deterioration;

        public Book() { }

        public Book(string name, ushort year, ushort pages, byte deterioration)
        {
            Id = Guid.NewGuid().ToString("N");
            Name = name;
            Year = year;
            Pages = pages;
            Deterioration = deterioration;
        }

        public object Clone()
        {
            return new Book(Name + "-copy", Year, Pages, Deterioration);
        }
    }
}
