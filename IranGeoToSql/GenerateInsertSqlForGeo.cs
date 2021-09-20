using System;
using System.Collections.Generic;
using System.Linq;

namespace IranGeoToSql
{
    public class GenerateInsertSqlForGeo
    {
        private const string TableName = "Locations";
        private const string Schema = "dbo";
        private const string CountryName = "ایران";
        private const string CountryLatinName = "Iran";
        private const int CountryLocationTypeId = 1;
        private const int ProvinceLocationTypeId = 2;
        private const int RegionLocationTypeId = 3;
        private const int AreaLocationTypeId = 4;
        private const int CityLocationTypeId = 5;
        private const int VillageLocationTypeId = 6;
        private readonly IEnumerable<GeoRow> _geoLocations;
        private readonly Dictionary<string, int> _codeToId = new Dictionary<string, int>();
        private int _tableId = 1;

        public GenerateInsertSqlForGeo(IEnumerable<GeoRow> geoLocations)
        {
            _geoLocations = geoLocations;
        }

        public IEnumerable<string> ToSqlInsert()
        {
            yield return $"INSERT INTO {Schema}.{TableName} " +
                         "(Id,Name,LatinName,NativeName,Latitude,Longitude,Altitude,ParentLocationId,LocationTypeId) " +
                         $"Values ({_tableId},N'{CountryName}',N'{CountryLatinName}',N'{CountryName}',NULL,NULL,NULL,NULL,{CountryLocationTypeId})";
            var orderedList = _geoLocations
                .OrderBy(l => l.Code.Length)
                .ThenBy(l => l.Code).ToList();
            foreach (var location in orderedList)
            {
                _tableId++;

                _codeToId.Add(location.Code, _tableId);

                var locationTypeId = GetLocationTypeId(location);

                int? parent = null;
                parent = GerParentCode((GeoLocationType)locationTypeId, location);
                // if (_codeToId.TryGetValue(location.Code, out var parentId))
                // {
                //     parent = parentId;
                // }

                yield return
                    $"INSERT INTO {Schema}.{TableName} " +
                    "(Id,Name,LatinName,NativeName,Latitude,Longitude,Altitude,ParentLocationId,LocationTypeId) " +
                    $"Values ({_tableId},N'{location.Name}',N'{location.Name}',N'{location.Name}',NULL,NULL,NULL,{parent},{locationTypeId})";
            }
        }

        private static int GetLocationTypeId(GeoRow row)
        {
            if (string.IsNullOrEmpty(row.RegionCode))
            {
                return ProvinceLocationTypeId;
            }

            if (string.IsNullOrEmpty(row.AreaCode))
            {
                return RegionLocationTypeId;
            }

            if (string.IsNullOrEmpty(row.CityCode))
            {
                return AreaLocationTypeId;
            }

            return string.IsNullOrEmpty(row.VillageCode) ? CityLocationTypeId : VillageLocationTypeId;
        }

        private int GerParentCode(GeoLocationType locationType, GeoRow row)
        {
            return locationType switch
            {
                GeoLocationType.Province => 1,
                GeoLocationType.Region => _codeToId[row.ProvinceCode],
                GeoLocationType.Area => _codeToId[$"{row.ProvinceCode}{row.RegionCode}"],
                GeoLocationType.City => _codeToId[$"{row.ProvinceCode}{row.RegionCode}{row.AreaCode}"],
                GeoLocationType.Village => _codeToId[$"{row.ProvinceCode}{row.RegionCode}{row.AreaCode}{row.CityCode}"],
                _ => throw new ArgumentOutOfRangeException(nameof(locationType), locationType, null)
            };
        }
    }

    public enum GeoLocationType
    {
        Province = 2,
        Region = 3,
        Area = 4,
        City = 5,
        Village = 6
    }
}