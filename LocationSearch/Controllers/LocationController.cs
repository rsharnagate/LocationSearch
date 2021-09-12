using LocationSearch.DBContext;
using LocationSearch.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSearch.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly NtsGeometryServices _ntsGeometryServices;

        /// <summary>
        /// The service constructor
        /// </summary>
        /// <param name="appDBContext">The injected application DB context</param>
        public LocationController(AppDBContext appDBContext, NtsGeometryServices ntsGeometryServices)
        {
            _context = appDBContext;
            _ntsGeometryServices = ntsGeometryServices;
        }

        /// <summary>
        /// Returns the nearby locations based on provided inputs.
        /// </summary>
        /// <param name="location">The current location of the user</param>
        /// <param name="maxDistance">The maximum distance from the current location</param>
        /// <param name="maxResults">The maximum number of locations to return</param>
        /// <exception cref="StatusCodeResult">200 - Returns success with list of locations</exception>
        /// <exception cref="StatusCodeResult">400 - Returns on invaid input</exception>
        /// <exception cref="StatusCodeResult">404 - Returns on empty result</exception>
        /// <exception cref="StatusCodeResult">500 - Returns on internal exception</exception>
        /// <returns>The list of nearby locations</returns>
        [HttpPost]
        public async Task<ActionResult<LocationResponse>> GetLocationsAsync([FromBody] Model.Location location, double maxDistance, int maxResults)
        {
            // Basic validation to save some time. Return 400 on invalid input
            if (location == null || location.Latitude == 0 || location.Longitude == 0 || maxDistance <= 0 || maxResults <= 0)
                return new ObjectResult("Please provide valid inputs")
                {
                    StatusCode = 400
                };

            try
            {
                var userLocation = _ntsGeometryServices.CreateGeometryFactory(4326).CreatePoint(new Coordinate(location.Longitude, location.Latitude));

                // Read the location details from the database.                
                var locationList = await _context.Tasks
                    .Where(l => l.GeoLocation.IsWithinDistance(userLocation, maxDistance)) // REQ1: It must be possible to set a maximum distance
                    .Take(maxResults) // REQ2: It must be possible to set a maximum number of results
                    .OrderBy(l => l.GeoLocation.Distance(userLocation)) // REQ3: Results should be ordered by distance
                    .Select(l => new LocationResponse(l.Id, l.Address, l.GeoLocation.Coordinate.Y, l.GeoLocation.Coordinate.X, l.GeoLocation.Distance(userLocation)))
                    .ToListAsync(); // Call is asynchronous to improve the performance as it won't block the thread.

                // Return 404 response on empty result
                if (locationList?.Count <= 0)
                    return new NotFoundResult();

                // Return 200 on success
                return new OkObjectResult(locationList);
            }
            catch (Exception) // We must capture the specific exceptions here
            {
                // Return 500 on internal exception. We must log the exception details to track down the issue. 
                return new StatusCodeResult(500);
            }
        }
    }
}
