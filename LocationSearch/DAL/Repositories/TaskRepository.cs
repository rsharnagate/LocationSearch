using LocationSearch.DBContext;
using LocationSearch.Interface;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSearch.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDBContext _context;
        private readonly NtsGeometryServices _ntsGeometryServices;

        /// <summary>
        /// Grab the dependencies from the DI container
        /// </summary>
        /// <param name="appDBContext"></param>
        /// <param name="ntsGeometryServices"></param>
        public TaskRepository(AppDBContext appDBContext, NtsGeometryServices ntsGeometryServices)
        {
            _context = appDBContext;
            _ntsGeometryServices = ntsGeometryServices;
        }

        /// <summary>
        /// Returns the nearby locations based on the provided input
        /// </summary>
        /// <param name="location">The user's current location</param>
        /// <returns>The list of nearby locations</returns>
        public async Task<List<Model.LocationResponse>> GetLocationsAsync(Model.Location location)
        {
            var userLocation = _ntsGeometryServices.CreateGeometryFactory(4326).CreatePoint(new Coordinate(location.Longitude, location.Latitude));

            // Read the location details from the database.                
            return await _context.Tasks
                .Where(l => l.GeoLocation.IsWithinDistance(userLocation, location.MaxDistance)) // REQ1: It must be possible to set a maximum distance                
                .OrderBy(l => l.GeoLocation.Distance(userLocation)) // REQ3: Results should be ordered by distance
                .Take(location.MaxResults) // REQ2: It must be possible to set a maximum number of results
                .Select(l => new Model.LocationResponse(l.Id, l.Address, l.GeoLocation.Coordinate.Y, l.GeoLocation.Coordinate.X, l.GeoLocation.Distance(userLocation)))
                .ToListAsync(); // Call is asynchronous to improve the performance as it won't block the thread.
        }

        public System.Threading.Tasks.Task InsertLocationAsync(Model.Task task)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateLocationAsync(Model.Task task)
        {
            throw new NotImplementedException();
        }
    }
}
