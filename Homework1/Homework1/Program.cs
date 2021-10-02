using System;
using System.Collections.Generic;

namespace Homework1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || !double.TryParse(args[0], out _))
            {
                Console.WriteLine("Only one numeric argument is allowed.");
                Console.WriteLine("Press Enter to terminate...");
                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                return;
            }

            List<Type> types = new() 
            { 
                typeof(byte), 
                typeof(sbyte), 
                typeof(int), 
                typeof(uint), 
                typeof(long),
                typeof(ulong), 
                typeof(float), 
                typeof(double) 
            };

            for (int i = 0; i < types.Count; i++)
            {
                Type type = types[i];
                Console.Write($"{i + 1}. {types[i].Name} - ");

                try
                {
                    checked
                    {
                        Convert.ChangeType(args[0], type);
                        Console.Write("true \n");
                    }
                }
                catch
                {
                    double maxValue = Convert.ToDouble(type.GetField("MaxValue").GetValue(type));
                    double minValue = Convert.ToDouble(type.GetField("MinValue").GetValue(type));
                    double value = Convert.ToDouble(args[0]);
                    double overlimit = value > maxValue ? value - maxValue : value + minValue;
                    Console.Write($"false (over limit = {overlimit}) \n");
                }
            }

            Console.WriteLine("Press Enter to terminate...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }
    }
}
