using System.Collections.Generic;
using System.IO;

namespace IranGeoToSql
{
    public class FileOutputGenerator
    {
        private readonly AppOptions _options;

        public FileOutputGenerator(AppOptions options)
        {
            _options = options;
        }

        public void Generate(IEnumerable<string> inputs)
        {
            if (!File.Exists(_options.OutputPath))
            {
                File.CreateText(_options.OutputPath);
            }

            using var tw = new StreamWriter(_options.OutputPath, true);
            foreach (var command in inputs)
            {
                tw.WriteLine(command);
            }

            tw.Flush();
            tw.Close();
        }
    }
}