using LocationSearch.Interface;
using LocationSearch.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationSearch.Controllers
{
    [ApiController]
    [Route("api/v1/locations")]    
    public class LocationController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// The service constructor
        /// </summary>
        public LocationController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationResponse>>> GetLocationsAsync([FromQuery]Model.Location location)
        {
            if (location == null || !ModelState.IsValid)
                return new ObjectResult("Please provide valid inputs")
                {
                    StatusCode = 400
                };                

            try
            {
                // Read the locations from the the database
                var locationList = await _taskRepository.GetLocationsAsync(location);

                // Return 404 response on empty result
                if (locationList == null || locationList.Count <= 0)
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
