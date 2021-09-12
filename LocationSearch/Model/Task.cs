using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationSearch.Model
{
    /// <summary>
    /// Represents the task at specific location
    /// </summary>
    [Table(name: "location")]
    public class Task
    {
        /// <summary>
        /// The task id
        /// </summary>
        [Key]
        public long Id { get; set; }
        
        /// <summary>
        /// The address of the task
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// The latitude of the task
        /// </summary>
        [Column(TypeName = "float")]
        [Required]
        public double Latitude { get; set; }

        /// <summary>
        /// The longitude of the task
        /// </summary>
        [Column(TypeName = "float")]
        [Required]
        public double Longitude { get; set; }

        /// <summary>
        /// The coordinates of the task
        /// </summary>
        [Column(TypeName = "geography")]
        [Required]
        public Point GeoLocation { get; set; }
    }
}
