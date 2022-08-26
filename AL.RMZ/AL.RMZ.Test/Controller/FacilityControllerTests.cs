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
    public class FacilityControllerTests
    {
        private FacilityRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public FacilityControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb_Facility")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new FacilityRepository(dbContext);

        }
        [Fact]
        public async void AddFacility_ValidData_ReturnOkResult()
        {
            //arrange
            var facilityController = new FacilityController(_repository);
            var City = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault();
            var facility = new FacilityRequest() { Name = "Bellandur", CityId = City.Id };

            //act
            var data = await facilityController.AddFacility(facility);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddFacility_InValidData_ReturnBadRequest()
        {
            //arrange
            var facilityController = new FacilityController(_repository);
            FacilityRequest facility = null;

            //act
            var data = await facilityController.AddFacility(facility);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddFacility_InValidData_Return_BadRequest()
        {
            //arrange
            var facilityController = new FacilityController(_repository);
            var facility = new FacilityRequest() { Name = null, CityId = 0 };

            //act
            var data = await facilityController.AddFacility(facility);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddFacility_ValidData_MatchResult()
        {
            //Arrange  
            var facilityController = new FacilityController(_repository);
            var City = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault();
            var facility = new FacilityRequest() { Name = "WhiteField", CityId = City.Id };


            //Act  
            var data = await facilityController.AddFacility(facility);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await facilityController.GetFacility((int?)okResult.Value);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var Facility = okResult2.Value.Should().BeAssignableTo<Facility>().Subject;

            Assert.Equal("WhiteField", Facility.Name);
            Assert.Equal(City.Id, Facility.CityId);
        }

        [Fact]
        public async void AddFacility_ExisitngData_ReturnOkResult_ShouldNotBeAdded()
        {
            //arrange
            var facilityController = new FacilityController(_repository);
            var City = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault();
            var facility = new FacilityRequest() { Name = "Majestic", CityId = City.Id };
            var countBeforeAddition = await dbContext.Facilities.CountAsync();
            //act
            var data = await facilityController.AddFacility(facility);
            var countAfterAddition = await dbContext.Facilities.CountAsync();

            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(countBeforeAddition, countAfterAddition);
        }

        #region Get By Id  

        [Fact]
        public async void GetFacilityById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var FacilityId = 2;

            //Act  
            var data = await controller.GetFacility(FacilityId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetFacilityById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var FacilityId = 10;

            //Act  
            var data = await controller.GetFacility(FacilityId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetFacilityById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            int? FacilityId = null;

            //Act  
            var data = await controller.GetFacility(FacilityId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetFacilityById_MatchResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            int? FacilityId = 1;

            //Act  
            var data = await controller.GetFacility(FacilityId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Facility = okResult.Value.Should().BeAssignableTo<Facility>().Subject;

            Assert.Equal("Majestic", Facility.Name);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetFacilities_Return_OkResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);

            //Act  
            var data = await controller.GetFacilities();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetFacilities_MatchResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);

            //Act  
            var data = await controller.GetFacilities();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Facility = okResult.Value.Should().BeAssignableTo<List<Facility>>().Subject;

            Assert.Equal("Carmelram", Facility[0].Name);
            Assert.Equal(1, Facility[0].CityId);
        }

        #endregion

        #region Get By CityId  

        [Fact]
        public async void GetFacilityByCityId_ValidData_OkResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var City = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault();

            //Act  
            var data = await controller.GetFacilitiesByCity(City.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetFacilityByCityId_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var CityId = 10;

            //Act  
            var data = await controller.GetFacilitiesByCity(CityId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetFacilityByCityId_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            int? CityId = null;

            //Act  
            var data = await controller.GetFacilitiesByCity(CityId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetFacilityByCityId_MatchResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var City = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault();

            //Act  
            var data = await controller.GetFacilitiesByCity(City.Id);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Facility = okResult.Value.Should().BeAssignableTo<List<Facility>>().Subject;

            Assert.Equal("Carmelram", Facility[0].Name);
        }

        #endregion


        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var facilityId = 1;
            var Facility = new FacilityRequest();

            Facility.Name = "Majestic";
            Facility.CityId = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateFacility(facilityId, Facility);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var facilityId = 1;
            var Facility = new FacilityRequest();

            Facility.Name = null;
            Facility.CityId = 0;
            //Act              
            var updatedData = await controller.UpdateFacility(facilityId, Facility);


            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var facilityId = 10;
            var Facility = new FacilityRequest();

            Facility.Name = "Majestic";
            Facility.CityId = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateFacility(facilityId, Facility);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(updatedData);
        }
        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequest()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var facilityId = 0;
            var Facility = new FacilityRequest();

            Facility.Name = "Majestic";
            Facility.CityId = dbContext.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateFacility(facilityId, Facility);

            //Assert  
            Assert.IsType<BadRequestResult>(updatedData);
        }

        #endregion



        #region Delete Facility  

        [Fact]
        public async void Delete_Facility_Return_OkResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var FacilityId = 2;

            //Act  
            var data = await controller.DeleteFacility(FacilityId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_Facility_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            var FacilityId = 5;

            //Act  
            var data = await controller.DeleteFacility(FacilityId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new FacilityController(_repository);
            int? FacilityId = null;

            //Act  
            var data = await controller.DeleteFacility(FacilityId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

