using System;
using System.Linq;

namespace IranGeoToSql
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start converting Iran's geo locations from excel to sql commands");
            var options = ReadConfiguration();
            var reader = new GeoExcelReader(options);
            var list = reader.Read().ToList();
            Console.WriteLine($"Excel contains {list.Count} rows");
            var converter = new GenerateInsertSqlForGeo(list);
            var commands = converter.ToSqlInsert().ToList();
            Console.WriteLine($"{list.Count} sql commands has generated.");
            var fileGenerator = new FileOutputGenerator(options);
            fileGenerator.Generate(commands);
            Console.WriteLine($"a file with name: {options.OutputPath} has generated at output folder.");
        }

        private static AppOptions ReadConfiguration()
        {
            var option = new AppOptions
            {
                FirstDataRowIndex = 3,
                ColumnsIndex = (1, 3, 5, 7, 9, 11),
                GeoExcelPath = "ExcelFile/GEO99.xlsx",
                OutputPath = "geo.txt"
            };

            return option;
        }
    }
}