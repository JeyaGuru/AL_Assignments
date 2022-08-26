using AL.RMZ.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AL.RMZ.Data;
using Microsoft.EntityFrameworkCore;
using AL.RMZ.Repository;
using AL.RMZ.Models;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace AL.RMZ.Test.Controller
{
    public class CityControllerTests
    {
        private CityRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public CityControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new CityRepository(dbContext);

        }
        [Fact]
        public async void AddCity_ValidData_ReturnOkResult()
        {
            //arrange
            var cityController = new CityController(_repository);
            var city = new CityRequest() { Name = "Delhi" };

            //act
            var data = await cityController.AddCity(city);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddCity_InValidData_ReturnBadRequest()
        {
            //arrange
            var cityController = new CityController(_repository);
            CityRequest city = null;

            //act
            var data = await cityController.AddCity(city);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddCity_ValidData_MatchResult()
        {
            //Arrange  
            var controller = new CityController(_repository);
            var city = new CityRequest() { Name = "Rajasthan" };

            //Act  
            var data = await controller.AddCity(city);

            //Assert  
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await controller.GetCity((int?)okResult.Value);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var City = okResult2.Value.Should().BeAssignableTo<City>().Subject;

            Assert.Equal("Rajasthan", City.Name);
        }

        [Fact]
        public async void AddCity_ExisitingData_ReturnOkResult_ShouldSkipDuplicate()
        {
            //arrange
            var cityController = new CityController(_repository);
            var city = new CityRequest() { Name = "Mumbai" };
            var cityCountBeforeAddition = await dbContext.Cities.CountAsync();

            //act
            var data = await cityController.AddCity(city);

            var cityCountAfterAddition = await dbContext.Cities.CountAsync();
            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(cityCountBeforeAddition, cityCountAfterAddition);
        }

        #region Get By Id  

        [Fact]
        public async void GetCityById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new CityController(_repository);
            var CityId = 2;

            //Act  
            var data = await controller.GetCity(CityId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetCityById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new CityController(_repository);
            var CityId = 10;

            //Act  
            var data = await controller.GetCity(CityId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetCityById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new CityController(_repository);
            int? CityId = null;

            //Act  
            var data = await controller.GetCity(CityId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetCityById_MatchResult()
        {
            //Arrange  
            var controller = new CityController(_repository);
            int? CityId = 1;

            //Act  
            var data = await controller.GetCity(CityId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var City = okResult.Value.Should().BeAssignableTo<City>().Subject;

            Assert.Equal("Bangalore", City.Name);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetCities_Return_OkResult()
        {
            //Arrange  
            var controller = new CityController(_repository);

            //Act  
            var data = await controller.GetCities();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetCities_MatchResult()
        {
            //Arrange  
            var controller = new CityController(_repository);

            //Act  
            var data = await controller.GetCities();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var City = okResult.Value.Should().BeAssignableTo<List<City>>().Subject;

            Assert.Equal("Bangalore", City[0].Name);
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CityController(_repository);

            var cityId = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault().Id;

            var City = new CityRequest();
            City.Name = "Bangalore1";

            //Act              
            var updatedData = await controller.UpdateCity(cityId, City);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new CityController(_repository);
            var cityId = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault().Id;

            var City = new CityRequest();
            City.Name = null;

            //Act              
            var data = await controller.UpdateCity(cityId, City);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new CityController(_repository);
            var cityId = 10;

            var City = new CityRequest();
            City.Name = "Gujarat";

            //Act              
            var data = await controller.UpdateCity(cityId, City);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequet()
        {
            //Arrange  
            var controller = new CityController(_repository);
            var cityId = 0;

            var City = new CityRequest();
            City.Name = "Gujarat";

            //Act              
            var data = await controller.UpdateCity(cityId, City);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion



        #region Delete City  

        [Fact]
        public async void Delete_City_Return_OkResult()
        {
            //Arrange  
            var controller = new CityController(_repository);
            var CityId = 2;

            //Act  
            var data = await controller.DeleteCity(CityId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_City_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CityController(_repository);
            var CityId = 5;

            //Act  
            var data = await controller.DeleteCity(CityId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CityController(_repository);
            int? CityId = null;

            //Act  
            var data = await controller.DeleteCity(CityId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

