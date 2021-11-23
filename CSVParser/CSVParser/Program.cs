using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CSVParser
{
    class Program
    {
        class FileStructure
        {
            public string Code { get; set; }
            public string Region { get; set; }
            public int Period { get; set; }
            public float Exports { get; set; }
            public float Imports { get; set; }
        }

        private static int YearWithMaxImport(List<FileStructure> records)
        {
            return records.Where(i => i.Imports == records.Where(k => k.Region == "Ukraine").Max(i => i.Imports))
                .Select(y => y.Period).FirstOrDefault();
        }
        private static int YearWithMinImport(List<FileStructure> records)
        {
            return records.Where(i => i.Imports == records.Where(k => k.Region == "Ukraine").Min(i => i.Imports))
                .Select(y => y.Period).FirstOrDefault();
        }
        private static int[] TotalImportsDivisibleBy2(List<FileStructure> records)
        {
            return records.GroupBy(x => x.Period)
                                .Select(gr => new
                                {
                                    Year = gr.Key,
                                    Sum = gr.Sum(item => item.Imports)
                                })
                                .Where(x => x.Sum % 2 == 0)
                                .Select(x => x.Year)
                                .ToArray();
        }

        private static Dictionary<string, float> AverageNumberOfImports(List<FileStructure> records, int period)
        {
            return records.Where(r => r.Period == period).GroupBy(r => r.Region)
                .ToDictionary(g => g.Key, g => g.Average(x => x.Imports));
        }

        private static Dictionary<string, float> MaximumNumberOfImportsForAGivenArea(List<FileStructure> records,
            string region, int from, int till)
        {
            return records.Where(r => r.Period >= from && r.Period <= till && r.Region == region)
                .GroupBy(r => r.Region).ToDictionary(g => g.Key, g => g.Max(x => x.Imports));
        }

        private static Dictionary<string, float> AverageImport(List<FileStructure> records)
        {
            return records.GroupBy(x => x.Region)
                           .Select(s => new { Key = s.Key.ToString(), Avg = s.Average(m => m.Imports) })
                           .ToDictionary(d => d.Key, d => d.Avg);
        }

        private static Dictionary<string, Dictionary<int, float>> SumImport(List<FileStructure> records)
        {
            return records.GroupBy(r => r.Region)
                .ToDictionary(g => g.Key,
                              g => g.ToLookup(d => d.Period, d => g.GroupBy(gb => gb.Period)
                                .Where(x => x.Key == d.Period).Select(s => s.Sum(y => y.Imports)).FirstOrDefault())
                                .ToDictionary(t => t.Key, t => t.Last()));
        }

        private static object OverUnderAverageGroups(List<FileStructure> records)
        {
            var averageImports = records.GroupBy(r => r.Region)
                .ToDictionary(g => g.Key, g => g.Average(y => y.Imports));

            var result =
                from r in records
                from a in averageImports
                group r by r.Period into rgroup
                select new
                {
                    OverAverage = rgroup.Where(g => g.Imports > averageImports.Where(i => i.Key == g.Region)
                        .Select(x => x.Value).FirstOrDefault()).Distinct(),
                    UnderAverage = rgroup.Where(g => g.Imports <= averageImports.Where(i => i.Key == g.Region)
                        .Select(x => x.Value).FirstOrDefault()).Distinct()
                };

            return result.ToList();
        }

        private static Tuple<int, string>[] MaxImportRegion(List<FileStructure> records)
        {
            return records.GroupBy(r => r.Period).AsEnumerable()
                .Select(g => new Tuple<int, string>(g.Key, g.OrderByDescending(i => i.Imports)
                .Where(x => x.Region != "Ukraine")
                .Select(y => y.Region).FirstOrDefault())).ToArray();
        }

        private static Tuple<int, string>[] MinImportRegion(List<FileStructure> records)
        {
            return records.GroupBy(r => r.Period).AsEnumerable()
                .Select(g => new Tuple<int, string>(g.Key, g.OrderBy(i => i.Imports)
                .Where(x => x.Region != "Ukraine")
                .Select(y => y.Region).FirstOrDefault())).ToArray();
        }

        private static IEnumerable<string> RegionsWithinLeapYears(List<FileStructure> records)
        {
            var leapYears = records.Where(r => DateTime.IsLeapYear(r.Period))
                .GroupBy(x => x.Period).Distinct().Count();

            return records.Where(r => DateTime.IsLeapYear(r.Period)
                && r.Imports > 250).GroupBy(gb => gb.Region).Where(x => x.Count() >= leapYears)
                .Select(y => y.Key).Distinct();
        }

        private static Dictionary<int, object> GroupImportsByYear(List<FileStructure> records)
        {
            return records.GroupBy(r => r.Period)
               .ToDictionary(d => d.Key, d => d.Select(s =>
               new { RegionName = s.Region, RegionValue = s.Imports }) as object);
        }

        private static List<string> RegionsBeforeSumy(List<FileStructure> records)
        {
            return records.OrderBy(r => r.Region).Select(r => r.Region).Distinct()
                .TakeWhile(x => x != "Sumy").ToList();
        }

        private static string[] RegionsWithUniqueImports(List<FileStructure> records)
        {
            return records.GroupBy(gb => gb.Region)
                .ToDictionary(g => g.Key, g => g.GroupBy(i => i.Period)
                .Select(x => x.FirstOrDefault()).Select(y => y.Imports))
                .Where(d => d.Value.Count() == d.Value.Distinct().Count()).Select(d => d.Key).ToArray();
        }

        private static Dictionary<string, int[]> YearsWithDecreasedImports(List<FileStructure> records)
        {
            return records.GroupBy(r => r.Region).ToDictionary(d => d.Key, d => d.GroupBy(g => g.Period)
                .Select(gb => gb.First()).Where((element, index) => element.Imports < d.GroupBy(g => g.Period)
                .Select(gb => gb.First()).ElementAt(index == 0 ? index : index - 1).Imports)
                .Select(s => s.Period).Distinct().ToArray());
        }

        private static Dictionary<string, float> SumImportByRegion(List<FileStructure> records)
        {
            return records.GroupBy(r => r.Region).ToDictionary(d => d.Key, d => d.Sum(x => x.Imports));
        }

        static void Main()
        {
            using var reader = new StreamReader(@".\resources\obsiagi.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = new List<FileStructure>();
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = new FileStructure
                {
                    Code = csv.GetField("Code"),
                    Region = csv.GetField("Region"),
                    Period = csv.GetField<int>("Period"),
                    Exports = csv.GetField("Exports") == "NA" ? 0 : csv.GetField<float>("Exports"),
                    Imports = csv.GetField("Imports") == "NA" ? 0 : csv.GetField<float>("Imports")
                };
                records.Add(record);
            }

            var maxImport = YearWithMaxImport(records);
            var minImport = YearWithMinImport(records);
            var yearsWithEvenSumImport = TotalImportsDivisibleBy2(records);
            var averageImportByYear = AverageNumberOfImports(records, 2010);
            var maxImportByPeriod = MaximumNumberOfImportsForAGivenArea(records, "Sumy", 2011, 2014);
            var averageImport = AverageImport(records);
            var sumImport = SumImport(records);
            var overUnderAverageGroups = OverUnderAverageGroups(records);
            var maxImportRegion = MaxImportRegion(records);
            var minImportRegion = MinImportRegion(records);
            var regionsWithinLeapYears = RegionsWithinLeapYears(records);
            var groupImportsByYear = GroupImportsByYear(records);
            var regionsBeforeSumy = RegionsBeforeSumy(records);
            var regionsWithUniqueImports = RegionsWithUniqueImports(records);
            var yearsWithDecreasedImports = YearsWithDecreasedImports(records);
            var sumImportByRegion = SumImportByRegion(records);

            Console.WriteLine(maxImport);
            Console.WriteLine(minImport);
            Console.ReadKey();
        }
    }
}