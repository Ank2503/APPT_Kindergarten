using System;


namespace Library.Materials
{
    class Magazine : Book
    {
        public readonly byte Number;
        public Magazine() { }

        public Magazine(string name, ushort year, ushort pages, byte deterioration, byte number)
            : base(name, year, pages, deterioration)
        {
            Number = number;
        }

        public static Magazine operator +(Magazine m1, Magazine m2)
        {
            return new Magazine($"{m1.Name}/{m2.Name}", Math.Max(m1.Year, m2.Year),
                (ushort)(m1.Pages + m2.Pages), (byte)Math.Ceiling((double)(m1.Deterioration + m2.Deterioration) / 2),
                (byte)Math.Ceiling((double)(m1.Number + m2.Number) / 2));
        }
    }
}
