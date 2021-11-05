using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4
{
    static class TaskMethods
    {
        //Год с наибольшим ... -> int 1
        public static int GetMaxImportYear(List<Capacity> records)
        {
            return records.GroupBy(x => x.Year)
                              .Select(gr => new
                              {
                                  Year = gr.Key,
                                  Sum = gr.Sum(item => item.Import)
                              })
                              .OrderByDescending(item => item.Sum)
                              .First()
                              .Year;
        }

        //... наименьшим количеством импорта по всем областям -> int 2
        public static int GetMinImportYear(List<Capacity> records)
        {
            return records.GroupBy(x => x.Year)
                              .Select(gr => new
                              {
                                  Year = gr.Key,
                                  Sum = gr.Sum(item => item.Import)
                              })                           
                              .OrderByDescending(item => item.Sum)
                              .Last()
                              .Year;
        }

        //Все годы, в которых сумарный импорт делится на 2 без остатка -> int[] 3
        public static int[] GetAvgYearBy2(List<Capacity> records)
        {
            return records.GroupBy(x => x.Year)
                              .Select(gr => new
                              {
                                  Year = gr.Key,
                                  Sum = gr.Sum(item => item.Import)
                              })
                              .Where(x => x.Sum % 2 == 0)
                              .Select(x => x.Year)
                              .ToArray();
        }

        //Среднее количество импорта по каждой из областей по указанному году -> Dictionary<string, int> 4
        public static Dictionary<string, float> getAvgRegions(List<Capacity> records, int year)
        {
            return records.Where(x => x.Year == year)
                .GroupBy(x => x.Region)
                .Select(s => new { Key = s.Key, Avg = s.Average(i => i.Import) })
                .ToDictionary(d => d.Key, d => d.Avg);
        }

        //Максимальное количество импорта для заданой области в заданном временном промежутке -> Dictionary<string, int> 5
        public static Dictionary<string, float> getRangeMaxImport(List<Capacity> records, int lowYear, int highYear, string region)
        {
            return records.Where(x => x.Region == region && x.Year >= lowYear && x.Year <= highYear)
                          .GroupBy(x => x.Year)
                          .Select(s => new { Key = s.Key.ToString(), Max = s.Max(m => m.Import) })
                          .ToDictionary(d => d.Key, d => d.Max);
        }

        //Среднее значение импорта по каждой из области за все года -> Dictionary<string, int> 6
        public static Dictionary<string, float> getRegionsMaxImport(List<Capacity> records)
        {
            return records.GroupBy(x => x.Region)
                          .Select(s => new { Key = s.Key.ToString(), Avg = s.Average(m => m.Import) })
                          .ToDictionary(d => d.Key, d => d.Avg);
        }

        //Суммарный импорт по всем областям за каждый из годов -> Dictionary<string, Dictionary<int, int>> 7
        public static Dictionary<string, Dictionary<int, float>> getAverageImport(List<Capacity> records)
        {
            return records.GroupBy(x => x.Region)
                .Select(s => new
                {
                    Key = s.Key,
                    Volume = s.GroupBy(x => x.Year)
                              .Select(x => new
                              {
                                  Year = x.Key,
                                  Volume = x.Average(m => m.Import - m.Export)
                              }).ToDictionary(d => d.Year, d => d.Volume)
                }).ToDictionary(d => d.Key, d => d.Volume);
        }

        //Сгруппировать области в 2 группы по объему импорта за все годы:
        //получить средний импорт по всем областям, в первую отнести все области, которые превышают среднее значение,
        //во вторую - которые не дотягивают до нее.
        //-> List<AnonymousObject{ OverAverage = string[], UnderAverage[string] }> 8
        public static List<object> getGroupedIntoTwo(List<Capacity> records)
        {
            float average = records.Average(x => x.Import - x.Export);

            return records.GroupBy(x => x.Region)
                          .Select(x => new
                          {
                              OverAverage = x.Where(x => (x.Import - x.Export) > average).ToArray(),
                              UnderAverage = x.Where(x => (x.Import - x.Export) < average).ToArray()
                          }).ToList<object>();

        }

        //Для каждого из годов выбрать область с максимальным объемом импорта -> (int, string)[] 9
        //public static Dictionary<int, string> minVolumeRegions(List<Capacity> records)
        //{
        //    return records.GroupBy(x => x.Year)
        //                  .Select(x => new
        //                  {
        //                      Key = x.Key,
        //                      Region = x.Select(x => x.Region),
        //                      Value = x.Max(x => x.Import)
        //                  })
        //                  .Select(x => new { x.Key, x.Region})                       
        //                  .ToDictionary(x=> x.Key, x=> x.Region);

        //}



        //Для каждого из годов выбрать область с минимальным объемом -> (int, string)[] 10
        //public static Dictionary<string, float> minVolumeRegions(List<Capacity> records)
        //{

        //}

        //Выбрать области, в которых в каждый из високостных годов объем ипорта превышал 250 -> IEnumerable<string> 11
        //How to add condition for year count
        public static IEnumerable<string> getVisokosnieYears(List<Capacity> records)
        {
            return records.GroupBy(x => x.Region)
                          .Where(x => x.Any(x => x.Import - x.Export > 250 && (x.Year % 4 == 0)))
                          .Select(x => x.Key);
        }

        //Сгруппировать все результаты по году в словарь, где год - ключ,
        //а значение - анонимный объект с именем области и объемом импорта.
        //-> Dictionary<int, AnonymousObject{ RegionName, RegionValue }> 12
        public static Dictionary<int, object> getGroupedKey(List<Capacity> records)
        {
            return records.GroupBy(x => x.Year)
                          .Select(s => new
                          {
                              Key = s.Key,
                              obj = s.Select(x => new
                              {
                                  RegionName = x.Region,
                                  RegionValue = x.Import
                              }) as object
                          })
                          .ToDictionary(x => x.Key, x => x.obj);
        }

        

        //Отсортировать все области по имени, выбрать области с именем до (не включая) Сумской. -> List<string> 13
        public static List<string> getSortedUntilSumy(List<Capacity> records)
        {
            return records.GroupBy(x => x.Region)
                          .OrderByDescending(x => x.Key).Reverse()
                          .Select(x => x.Key)
                          .TakeWhile(x => x !="Sumy")
                          .ToList();
        }      

        //Найти области у которых значения по импорту за все годы уникальные
        //(если есть 2014 и 2017 год с одинаковыми показателями,
        //то такую область включать в результат не нужно) -> string[] 14
        //запрос неправильный, переделать
        public static string[] getDistinctCount(List<Capacity> records)
        {
            return records.GroupBy(x => x.Region)
                            .Where(w => w.Select(i => i.Import).Distinct().Count() == w.Select(i => i.Import).Count())
                            .Select(x => x.Key)
                            .ToArray();
        }

        //Для каждой области вывести года, где импорт за прошлый год был выше,
        //чем за текущий(определение тенденции уменьшения импорта) либо не выводить,
        //если такой тенденции не было. -> Dictionary<string, int[]> 15
        //Не доделано
        public static string[] getIndexTask(List<Capacity> records)
        {
            return records.GroupBy(x => x.Region)
                          .Select(x => new
                          {
                              Key = x.Key,
                              isHigher = x.Skip(1).Zip(x, (curr, prev) => curr.Import > prev.Import)
                          })
                          .Select(x => x.Key)
                          .ToArray();
                          
        }

        //Метод, в который через аргумент передается string[] с именем областей
        //и возвращается Dictionary<string, int> с суммой импорта только по этим областям
        //за все годы -> Dictionary<string, int> 16
        public static Dictionary<string, float> getSumImportForRegions(List<Capacity> records, string[] regions)
        {
            return records.GroupBy(x => x.Region)
                .Where(x => regions.Any(s => x.Key.Contains(s)))
                .Select(s => new
                {
                    Key = s.Key,
                    Volume = s.Sum(x => x.Import)
                })
                .ToDictionary(d => d.Key, d => d.Volume);
        }

    }
}

