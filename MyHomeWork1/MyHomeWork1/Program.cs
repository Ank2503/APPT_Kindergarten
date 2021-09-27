using System;
using System.Collections.Generic;

namespace MyHomeWork1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var num = args[0];
            if (args.Length != 1 || !double.TryParse(args[0], out _))
            {
                Console.WriteLine("Please try some number");
                Console.ReadLine();
                return;
            }

            List<Type> typesList = new List<Type>() { typeof(byte), typeof(sbyte), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double) };

            for (int i = 0; i < typesList.Count; i++)
            {
                Console.Write($"{i + 1}. {typesList[i].Name} - ");
                try
                {
                    checked
                    {
                        Convert.ChangeType(args[0], typesList[i]);
                        Console.Write("true \n");
                    }
                }
                catch
                {
                    double inputValue = Convert.ToDouble(args[0]);
                    double minLimitValue = Convert.ToDouble(typesList[i].GetField("MinValue").GetValue(null));
                    double maxLimitValue = Convert.ToDouble(typesList[i].GetField("MaxValue").GetValue(null)) + 1;
                    double Limit = inputValue > maxLimitValue ? inputValue - maxLimitValue : minLimitValue - inputValue;
                    if(inputValue > maxLimitValue)
                    {
                        Console.Write($"false (over limit = {Limit}) \n");
                    }
                    else if(inputValue < maxLimitValue)
                    {
                        Console.Write($"false (under limit = {Limit}) \n");
                    }
                }
            };
            Console.ReadLine();
        }
    }
}
