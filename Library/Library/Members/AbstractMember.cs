using System;
using Library.Enums;

namespace Library.Members
{
    public abstract class AbstractMember : IComparable
    {
        public readonly string Name;
        public readonly byte Age;
        public readonly Gender Gender;
        public readonly string Role;

        public AbstractMember(string name, byte age, Gender gender, string role)
        {
            Name = name;
            Age = age;
            Gender = gender;
            Role = role;
        }

        public int CompareTo(object obj)
        {
            if (obj is AbstractMember p)
                return Name.CompareTo(p.Name);
            else
                throw new Exception("Can't compare person objects");
        }
    }
}
