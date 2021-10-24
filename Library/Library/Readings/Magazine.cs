using System;

namespace Library.Readings
{
    public class Magazine : Book
    {
        public readonly byte Number;

        public Magazine() { }

        public Magazine(string name, ushort year, ushort pages, byte wearout, byte number) 
            : base(name, year, pages, wearout)
        {
            Number = number;
        }

        public static Magazine operator +(Magazine m1, Magazine m2)
        {
            return new Magazine($"{m1.Name}/{m2.Name}", Math.Max(m1.Year, m2.Year),
                (ushort)(m1.Pages + m2.Pages), (byte)Math.Ceiling((double)(m1.Wearout + m2.Wearout) / 2),
                (byte)Math.Ceiling((double)(m1.Number + m2.Number) / 2));
        }
    }
}
