
namespace LocationSearch.Model
{
    /// <summary>
    /// Represents the task location in the provided csv file.
    /// </summary>
    public class CsvLocation
    {
        /// <summary>
        /// The address of the task
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The latitide of the task
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// The longitude of the task
        /// </summary>
        public double Longitude { get; set; }
    }
}
