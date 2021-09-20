namespace IranGeoToSql
{
    public class GeoRow
    {
        public GeoRow(string provinceCode, string regionCode, string areaCode, string cityCode, string villageCode,
            string name)
        {
            ProvinceCode = provinceCode?.Trim();
            RegionCode = regionCode?.Trim();
            AreaCode = areaCode?.Trim();
            CityCode = cityCode?.Trim();
            VillageCode = villageCode?.Trim();
            Name = name;
        }

        /// <summary>
        /// استان
        /// </summary>
        public string ProvinceCode { get; }

        /// <summary>
        /// شهرستان
        /// </summary>
        public string RegionCode { get; }

        /// <summary>
        /// بخش
        /// </summary>
        public string AreaCode { get; }

        /// <summary>
        /// دهستان
        /// </summary>
        public string CityCode { get; }

        /// <summary>
        /// روستا
        /// </summary>
        public string VillageCode { get; }

        public string Name { get; }

        public string Code => $"{ProvinceCode}{RegionCode}{AreaCode}{CityCode}{VillageCode}";
    }
}