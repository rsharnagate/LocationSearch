using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSearch.Interface
{
    public interface ITaskRepository
    {
        /// <summary>
        /// Returns the nearby locations based on the provided input
        /// </summary>
        /// <param name="location">The user's current location</param>
        /// <returns>The list of nearby locations</returns>
        Task<List<Model.LocationResponse>> GetLocationsAsync(Model.Location location);

        /// <summary>
        /// Insert the new task in the database
        /// </summary>
        /// <param name="task">The task to insert in the database</param>
        /// <returns></returns>
        Task InsertLocationAsync(Model.Task task);

        /// <summary>
        /// Update the existing task in the database
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task UpdateLocationAsync(Model.Task task);        
    }
}
