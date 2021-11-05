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
        static void Main()
        {
            var records = GetCSVData(@".\Resources\26-regionalni-obsiagi-zovnishnoyi-torgivli-tovarami-mln-dol-ssha.csv");

            var maxImport = GetMaxImportYear(records);
            var minImport = GetMinImportYear(records);
            var yearsWithEvenSumImport = GetYearsWithEvenSumImport(records);
            var averageImportByYear = GetAverageImportByYear(records, 2010);
            var maxImportByPeriod = GetMaxImportByPeriod(records, "Sumy", 2011, 2014);
            var averageImport = GetAverageImport(records);
            var sumImport = GetSumImport(records);
            var overUnderAverageGroups = GetOverUnderAverageGroups(records);
            var maxImportRegion = GetMaxImportRegion(records);
            var minImportRegion = GetMinImportRegion(records);
            var regionsWithinLeapYears = GetRegionsWithinLeapYears(records);
            var groupImportsByYear = GroupImportsByYear(records);
            var regionsBeforeSumy = GetRegionsBeforeSumy(records);
            var regionsWithUniqueImports = GetRegionsWithUniqueImports(records);
            var yearsWithDecreasedImports = GetYearsWithDecreasedImports(records);
            var sumImportByRegion = GetSumImportByRegion(records);

            Console.ReadKey();
        }

        private static List<CSVStructure> GetCSVData(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File doesn't exist!");

            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = new List<CSVStructure>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new CSVStructure
                    {
                        Code = csv.GetField("Code"),
                        Region = csv.GetField("Region"),
                        Period = csv.GetField<int>("Period"),
                        Exports = csv.GetField("Exports") == "NA" ? 0 : csv.GetField<float>("Exports"),
                        Imports = csv.GetField("Imports") == "NA" ? 0 : csv.GetField<float>("Imports")
                    };
                    records.Add(record);
                }

                return records;
            }
        }

        private static int GetMaxImportYear(List<CSVStructure> records)
        {
            return records.Where(r => r.Imports == records.Where(i => i.Region == "Ukraine").Max(r => r.Imports))
                .Select(s => s.Period).FirstOrDefault();
        }

        private static int GetMinImportYear(List<CSVStructure> records)
        {
            return records.Where(r => r.Imports == records.Where(i => i.Region == "Ukraine").Min(r => r.Imports))
                .Select(s => s.Period).FirstOrDefault();
        }

        private static int[] GetYearsWithEvenSumImport(List<CSVStructure> records)
        {
            return records.Where(r => r.Code == "0000000000" &&
                r.Imports % 2 == 0).Distinct().Select(s => s.Period).ToArray();
        }

        private static Dictionary<string, float> GetAverageImportByYear(List<CSVStructure> records, int period)
        {
            return records.Where(r => r.Period == period).GroupBy(r => r.Region)
                .ToDictionary(g => g.Key, g => g.Average(x => x.Imports));
        }

        private static Dictionary<string, float> GetMaxImportByPeriod(List<CSVStructure> records,
            string region, int from, int till)
        {
            return records.Where(r => r.Period >= from && r.Period <= till && r.Region == region)
                .GroupBy(r => r.Region).ToDictionary(g => g.Key, g => g.Max(x => x.Imports));
        }

        private static Dictionary<string, float> GetAverageImport(List<CSVStructure> records)
        {
            return records.GroupBy(r => r.Region)
                .ToDictionary(g => g.Key, g => g.Average(x => x.Imports));
        }

        private static Dictionary<string, Dictionary<int, float>> GetSumImport(List<CSVStructure> records)
        {
            return records.GroupBy(r => r.Region)
                .ToDictionary(g => g.Key,
                              g => g.ToLookup(d => d.Period, d => g.GroupBy(gb => gb.Period)
                                .Where(x => x.Key == d.Period).Select(s => s.Sum(y => y.Imports)).FirstOrDefault())
                                .ToDictionary(t => t.Key, t => t.Last()));
        }

        private static object GetOverUnderAverageGroups(List<CSVStructure> records)
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

        private static Tuple<int, string>[] GetMaxImportRegion(List<CSVStructure> records)
        {
            return records.GroupBy(r => r.Period).AsEnumerable()
                .Select(g => new Tuple<int, string>(g.Key, g.OrderByDescending(i => i.Imports)
                .Where(x => x.Region != "Ukraine")
                .Select(y => y.Region).FirstOrDefault())).ToArray();
        }

        private static Tuple<int, string>[] GetMinImportRegion(List<CSVStructure> records)
        {
            return records.GroupBy(r => r.Period).AsEnumerable()
                .Select(g => new Tuple<int, string>(g.Key, g.OrderBy(i => i.Imports)
                .Where(x => x.Region != "Ukraine")
                .Select(y => y.Region).FirstOrDefault())).ToArray();
        }

        private static IEnumerable<string> GetRegionsWithinLeapYears(List<CSVStructure> records)
        {
            var leapYears = records.Where(r => DateTime.IsLeapYear(r.Period))
                .GroupBy(x => x.Period).Distinct().Count();

            return records.Where(r => DateTime.IsLeapYear(r.Period)
                && r.Imports > 250).GroupBy(gb => gb.Region).Where(x => x.Count() >= leapYears)
                .Select(y => y.Key).Distinct();
        }

        private static Dictionary<int, object> GroupImportsByYear(List<CSVStructure> records)
        {
            return records.GroupBy(r => r.Period)
               .ToDictionary(d => d.Key, d => d.Select(s =>
               new { RegionName = s.Region, RegionValue = s.Imports }) as object);            
        }

        private static List<string> GetRegionsBeforeSumy(List<CSVStructure> records)
        {
            return records.OrderBy(r => r.Region).Select(r => r.Region).Distinct()
                .TakeWhile(x => x != "Sumy").ToList();            
        }

        private static string[] GetRegionsWithUniqueImports(List<CSVStructure> records)
        {
            return records.GroupBy(gb => gb.Region)
                .ToDictionary(g => g.Key, g => g.GroupBy(i => i.Period)
                .Select(x => x.FirstOrDefault()).Select(y => y.Imports))
                .Where(d => d.Value.Count() == d.Value.Distinct().Count()).Select(d => d.Key).ToArray();
        }

        private static Dictionary<string, int[]> GetYearsWithDecreasedImports(List<CSVStructure> records)
        {
            // Just for check during debugging
            var data = records.GroupBy(r => r.Region).
                ToDictionary(d => d.Key, d => d.GroupBy(g => g.Period).Select(gb => gb.First()).ToList());

            return records.GroupBy(r => r.Region).ToDictionary(d => d.Key, d => d.GroupBy(g => g.Period)
                .Select(gb => gb.First()).Where((element, index) => element.Imports < d.GroupBy(g => g.Period)
                .Select(gb => gb.First()).ElementAt(index == 0 ? index : index - 1).Imports)
                .Select(s => s.Period).Distinct().ToArray());            
        }

        private static Dictionary<string, float> GetSumImportByRegion(List<CSVStructure> records)
        {
            return records.GroupBy(r => r.Region).ToDictionary(d => d.Key, d => d.Sum(x => x.Imports));           
        }
    }

    class CSVStructure
    {
        public string Code { get; set; }
        public string Region { get; set; }
        public int Period { get; set; }
        public float Exports { get; set; }
        public float Imports { get; set; }
    }
}
