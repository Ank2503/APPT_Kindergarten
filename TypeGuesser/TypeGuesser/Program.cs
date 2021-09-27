using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace TypeGuesser
{
    class Program
    {
        static private readonly Dictionary<Type, string> _types = new Dictionary<Type, string>
            {
                {typeof(byte), "byte"},
                {typeof(sbyte), "sbyte"},
                {typeof(short), "short"},
                {typeof(ushort), "ushort"},
                {typeof(int), "int"},
                {typeof(uint), "uint"},
                {typeof(long), "long"},
                {typeof(ulong), "ulong"},
                {typeof(float), "float"},
                {typeof(double), "double"}
            };

        static private string IsFit(string num, Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);

            if (converter.IsValid(num))
                return "true";

            if (num.StartsWith("-"))
            {
                var diff = Convert.ToDouble(type.GetField("MinValue").GetValue(null)) - Convert.ToDouble(num);
                return $"false (under limit = {diff})";
            }
            else 
            {
                var diff = Convert.ToDouble(num) - Convert.ToDouble(type.GetField("MaxValue").GetValue(null));
                return $"false (over limit = {diff})";
            }             
        }

        static private void WaitForEnter()
        {
            Console.WriteLine("\nPress Enter to exit...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            Environment.Exit(0);
        }

        static private bool IsNumber(string input)
        {
            return Regex.IsMatch(input, @"^(-?|\+?)\d+\.?\d*$");
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Wrong input. Please use single argument.");
                WaitForEnter();
            }
            
            if (!IsNumber(args[0]))
            {
                Console.WriteLine("Input is not a number at all");
                WaitForEnter();
            }

            var i = 1;
            foreach (var t in _types)
            {
                Console.WriteLine($"{i}. {t.Value} - {IsFit(args[0], t.Key)}");
                i++;
            }

            WaitForEnter();
        }
    }
}
