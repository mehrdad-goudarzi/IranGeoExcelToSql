using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace IranGeoToSql
{
    public class GeoExcelReader
    {
        private readonly AppOptions _appOptions;

        public GeoExcelReader(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public IEnumerable<GeoRow> Read()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(new FileInfo(_appOptions.GeoExcelPath));
            var firstSheet = package.Workbook.Worksheets[0];
            for (var row = 3; row < firstSheet.Cells.Rows; row++)
            {
                var provinceCode = firstSheet.GetValue<string>(row, _appOptions.ColumnsIndex.Province);
                var regionCode = firstSheet.GetValue<string>(row, _appOptions.ColumnsIndex.Region);
                var areaCode = firstSheet.GetValue<string>(row, _appOptions.ColumnsIndex.Area);
                var cityCode = firstSheet.GetValue<string>(row, _appOptions.ColumnsIndex.City);
                var villageCode = firstSheet.GetValue<string>(row, _appOptions.ColumnsIndex.Village);
                var name = firstSheet.GetValue<string>(row, _appOptions.ColumnsIndex.Name);

                var geoRow = new GeoRow(provinceCode, regionCode, areaCode, cityCode, villageCode, name);

                if (!string.IsNullOrEmpty(geoRow.Code))
                {
                    yield return geoRow;
                }
            }
        }
    }
}