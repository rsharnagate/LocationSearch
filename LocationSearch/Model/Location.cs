using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LocationSearch.Model
{
    public class Location
    {
        public Location()
        {

        }

        public Location(double latitude, double longitude, double maxDistance, int maxResults)
        {
            Latitude = latitude;
            Longitude = longitude;
            MaxDistance = maxDistance;
            MaxResults = maxResults;
        }

        [Required]
        [Range(-90, 90, ErrorMessage = "Invalid latitude value. It must be in the range of -90 to 90.")]
        public double Latitude { get; set; }
        
        [Required]
        [Range(-180, 180, ErrorMessage = "Invalid longitude value. It must be in the range of -180 to 180.")]
        public double Longitude { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Maximum distance must be grater than 1 meter")]
        public double MaxDistance { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Maximum results must be greater than 0")]
        public int MaxResults { get; set; }

    }
}
