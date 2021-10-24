using Library.Enums;
using System;

namespace Library.Persons
{
    public abstract class AbstractPerson : IComparable
    {
        public readonly string Name;
        public readonly byte Age;
        public readonly Gender Gender;

        public AbstractPerson(string name, byte age, Gender gender)
        {
            Name = name;
            Age = age;
            Gender = gender;
        }

        public int CompareTo(object obj)
        {
            AbstractPerson p = obj as AbstractPerson;

            if (p != null)
                return this.Name.CompareTo(p.Name);
            else
                throw new Exception("Can't compare person objects");
        }
    }
}
