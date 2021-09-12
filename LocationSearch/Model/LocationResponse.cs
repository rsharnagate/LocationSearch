using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSearch.Model
{
    public class LocationResponse : CsvLocation
    {
        public LocationResponse(long id, string address, double latitude, double longitude, double distance)
        {
            Id = id;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
            Distance = distance;
        }

        /// <summary>
        /// The record unique id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Distance between user and the task location in meters
        /// </summary>
        public double Distance { get; set; }
    }
}
