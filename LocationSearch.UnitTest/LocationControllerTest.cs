using LocationSearch.Controllers;
using LocationSearch.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocationSearch.UnitTest
{
    public class Tests
    {
        private Mock<ITaskRepository> _mockTaskRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
        }

        [Test]
        public async Task GetLocationsAsync_WhenInvalidInputs()
        {
            var controller = new LocationController(_mockTaskRepository.Object);
            var response = await controller.GetLocationsAsync(null);

            Assert.AreEqual(400, ((ObjectResult)response.Result).StatusCode);
        }

        [Test]
        public async Task GetLocationsAsync_WhenEmptyResult()
        {
            var controller = new LocationController(_mockTaskRepository.Object);
            var response = await controller.GetLocationsAsync(new Model.Location(51.2165425, 4.4778534, 1, 1));

            Assert.AreEqual(404, ((StatusCodeResult)response.Result).StatusCode);
        }

        [Test]
        public async Task GetLocationsAsync_WhenSuccess()
        {
            // Mock the GetLocationsAsync() repository call to return the fake data
            var fakeLocationList = new List<Model.LocationResponse>
            {
                new Model.LocationResponse(1, "Fake Address 1", 12.12345, 34.12345, 1234),
                new Model.LocationResponse(2, "Fake Address 2", 56.12345, 78.12345, 1234)
            };
            _mockTaskRepository.Setup(m => m.GetLocationsAsync(It.IsAny<Model.Location>()))
                .ReturnsAsync(fakeLocationList);

            var controller = new LocationController(_mockTaskRepository.Object);
            var response = await controller.GetLocationsAsync(new Model.Location(52.2165425, 5.4778534, 1, 1));

            Assert.AreEqual(200, ((OkObjectResult)response.Result).StatusCode);
        }

        [Test]        
        public async Task GetLocationsAsync_WhenExceptionThrown()
        {
            // Mock the GetLocationsAsync() repository call to throw an exception.
            _mockTaskRepository.Setup(m => m.GetLocationsAsync(It.IsAny<Model.Location>()))
                .ThrowsAsync(new Exception());

            var controller = new LocationController(_mockTaskRepository.Object);
            var response = await controller.GetLocationsAsync(new Model.Location(52.2165425, 5.4778534, 1, 1));

            Assert.AreEqual(500, ((StatusCodeResult)response.Result).StatusCode);
        }
    }
}