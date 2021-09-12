using LocationSearch.Controllers;
using LocationSearch.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using System.Threading.Tasks;

namespace LocationSearch.UnitTest
{
    public class Tests
    {
        private AppDBContext _locationDbContext = null;

        [OneTimeSetUp]
        public void Setup()
        {
            // Setup DBContext with memory database

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "location").Options;

            _locationDbContext = new AppDBContext(options);
            _locationDbContext.Tasks.Add(new Model.Task
            {
                Id = 1,
                Address = "Address 123",
                Latitude = 52.2165425,
                Longitude = 5.4778534,
                GeoLocation = new Point(new Coordinate(5.4778534, 52.2165425))
            });
            _locationDbContext.SaveChanges();
        }

        [Test]
        public async Task GetLocationsAsync_WhenInvalidInputs()
        {
            var controller = new LocationController(_locationDbContext, null);
            var response = await controller.GetLocationsAsync(null, 1, 1);

            Assert.AreEqual(400, ((ObjectResult)response.Result).StatusCode);
        }

        [Test]
        public async Task GetLocationsAsync_WhenEmptyResult()
        {
            var ntsGeometryServices = new NtsGeometryServices();
            var controller = new LocationController(_locationDbContext, ntsGeometryServices);
            var response = await controller.GetLocationsAsync(new Model.Location(51.2165425, 4.4778534), 1, 1);

            Assert.AreEqual(404, ((StatusCodeResult)response.Result).StatusCode);
        }

        [Test]
        public async Task GetLocationsAsync_WhenSuccess()
        {
            var ntsGeometryServices = new NtsGeometryServices();
            var controller = new LocationController(_locationDbContext, ntsGeometryServices);
            var response = await controller.GetLocationsAsync(new Model.Location(52.2165425, 5.4778534), 1, 1);

            Assert.AreEqual(200, ((OkObjectResult)response.Result).StatusCode);
        }

        [Test]        
        public async Task GetLocationsAsync_WhenExceptionThrown()
        {
            var controller = new LocationController(_locationDbContext, null);
            var response = await controller.GetLocationsAsync(new Model.Location(52.2165425, 5.4778534), 1, 1);

            Assert.AreEqual(500, ((StatusCodeResult)response.Result).StatusCode);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (_locationDbContext != null)
                _locationDbContext.Dispose();
        }
    }
}