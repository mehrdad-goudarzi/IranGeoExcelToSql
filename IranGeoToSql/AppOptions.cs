namespace IranGeoToSql
{
    public class AppOptions
    {
        public int FirstDataRowIndex { get; set; }
        public (int Province, int Region, int Area, int City, int Village, int Name) ColumnsIndex { get; set; }
        public string GeoExcelPath { get; set; }
        public string OutputPath { get; set; }
    }
}