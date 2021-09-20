using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Homework1_Kurochkin
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 && !double.TryParse(args[0], out _))
            {
                Console.WriteLine("Wrong input value!");
                Console.ReadLine();
                return;
            }

            Tuple<byte, sbyte, int, uint, long, ulong, float, Tuple<double>> numberTypes =
                new(0, 0, 0, 0, 0, 0, 0, new Tuple<double>(0));

            ITuple iT = numberTypes as ITuple;

            for (int i = 0; i < iT.Length; i++)
            {
                Type type = iT[i].GetType();
                Console.Write($"{i+1}. {type.Name} - ");
                try
                {
                    checked
                    {
                        Convert.ChangeType(args[0], type);
                        Console.Write("true\n");
                    }
                }
                catch (Exception)
                {
                    double maxValue = Convert.ToDouble(type.GetField("MaxValue").GetValue(null)) + 1;
                    double minValue = Convert.ToDouble(type.GetField("MinValue").GetValue(null)) - 1;
                    double value = Convert.ToDouble(args[0]);
                    double resultDiff = value > maxValue ? value - maxValue : value + minValue;
                    Console.Write($"false (over limit = {resultDiff})\n");
                }             
            }
            Console.ReadLine();
        }
    }
}
