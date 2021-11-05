using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Homework4
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader(@"C:\\work\\homework4.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = new List<Capacity>();
                csv.Read();
                csv.ReadHeader();

                while(csv.Read())
                {
                    var record = new Capacity
                    {
                        Code = csv.GetField("Code"),
                        Region = csv.GetField("Region"),
                        Year = csv.GetField<int>("Year"),
                        Import = csv.GetField("Import") == "NA" ? 0 : csv.GetField<float>("Import"),
                        Export = csv.GetField("Export") == "NA" ? 0 : csv.GetField<float>("Export")
                    };
                    records.Add(record);
                }

                int task1 = TaskMethods.GetMaxImportYear(records);
                int task2 = TaskMethods.GetMinImportYear(records);
                Console.WriteLine("MAX " + task1 + " MIN " + task2);

                int[] task3 = TaskMethods.GetAvgYearBy2(records);
                Console.WriteLine();
                foreach (int i in task3)
                {
                    Console.WriteLine(i);
                }
                Console.WriteLine();

                var task4 = TaskMethods.getAvgRegions(records, 2021);
                foreach (var p in task4)
                {
                    Console.WriteLine(p.Key + " " + p.Value);
                }
                Console.WriteLine();

                var task5 = TaskMethods.getRangeMaxImport(records, 2021, 2022, "Test_City");
                foreach (var p in task5)
                {
                    Console.WriteLine(p.Key + " " + p.Value);
                }
                Console.WriteLine();

                var task6 = TaskMethods.getRegionsMaxImport(records);
                foreach (var p in task6)
                {
                    Console.WriteLine(p.Key + " " + p.Value);
                }
                Console.WriteLine();

                var task7 = TaskMethods.getAverageImport(records);
                foreach(var i in task7)
                {
                    Console.WriteLine(i.Key);
                    foreach(var j in i.Value)
                    {
                        Console.WriteLine(j.Key + " " + j.Value);
                    }
                }

                var task8 = TaskMethods.getGroupedIntoTwo(records);

                var task12 = TaskMethods.getGroupedKey(records);
                foreach(var p in task12)
                {
                    Console.WriteLine(p.Key + " " +p.Value);
                }

                var task13 = TaskMethods.getSortedUntilSumy(records);
                foreach(var p in task13)
                {
                    Console.WriteLine(p);
                }

                string[] task16_test = new string[] { "Test_City" };
                var task16 = TaskMethods.getSumImportForRegions(records, task16_test);
                foreach(var i in task16)
                {
                    Console.WriteLine(i.Key + " " + i.Value);
                }
            }           
        }
    }
}
