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
    public class BuildingControllerTests
    {
        private BuildingRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public BuildingControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb_Building")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new BuildingRepository(dbContext);

        }
        [Fact]
        public async void AddBuilding_ValidData_ReturnOkResult()
        {
            //arrange
            var buildingController = new BuildingController(_repository);
            var Facility = dbContext.Facilities.Where(x => x.Name == "Majestic").FirstOrDefault();
            var building = new BuildingRequest() { Name = "Building_001", FacilityId = Facility.Id };

            //act
            var data = await buildingController.AddBuilding(building);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddBuilding_InValidData_ReturnBadRequest()
        {
            //arrange
            var buildingController = new BuildingController(_repository);
            BuildingRequest building = null;

            //act
            var data = await buildingController.AddBuilding(building);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddBuilding_InValidData_Return_BadRequest()
        {
            //arrange
            var buildingController = new BuildingController(_repository);
            var building = new BuildingRequest() { Name = null, FacilityId = 0 };

            //act
            var data = await buildingController.AddBuilding(building);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddBuilding_ValidData_MatchResult()
        {
            //Arrange  
            var buildingController = new BuildingController(_repository);
            var Facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();
            var building = new BuildingRequest() { Name = "Building_002", FacilityId = Facility.Id };


            //Act  
            var data = await buildingController.AddBuilding(building);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await buildingController.GetBuilding((int?)okResult.Value);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var Building = okResult2.Value.Should().BeAssignableTo<Building>().Subject;

            Assert.Equal("Building_002", Building.Name);
            Assert.Equal(Facility.Id, Building.FacilityId);
        }

        [Fact]
        public async void AddBuilding_ExisitingData_ReturnOkResult()
        {
            //arrange
            var buildingController = new BuildingController(_repository);
            var Facility = dbContext.Facilities.Where(x => x.Name == "Majestic").FirstOrDefault();
            var building = new BuildingRequest() { Name = "Building002", FacilityId = Facility.Id };
            var buildingCountBeforeAddition =  dbContext.Buildings.Count();

            //act
            var data = await buildingController.AddBuilding(building);
            var buildingCountAfterAddition = dbContext.Buildings.Count();

            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(buildingCountBeforeAddition, buildingCountAfterAddition);
        }

        #region Get By Id  

        [Fact]
        public async void GetBuildingById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var BuildingId = 2;

            //Act  
            var data = await controller.GetBuilding(BuildingId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetBuildingById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var BuildingId = 10;

            //Act  
            var data = await controller.GetBuilding(BuildingId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetBuildingById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            int? BuildingId = null;

            //Act  
            var data = await controller.GetBuilding(BuildingId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetBuildingById_MatchResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            int? BuildingId = 1;

            //Act  
            var data = await controller.GetBuilding(BuildingId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Building = okResult.Value.Should().BeAssignableTo<Building>().Subject;

            Assert.Equal("Building001", Building.Name);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetBuildings_Return_OkResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);

            //Act  
            var data = await controller.GetBuildings();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetBuildings_MatchResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);

            //Act  
            var data = await controller.GetBuildings();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Building = okResult.Value.Should().BeAssignableTo<List<Building>>().Subject;

            Assert.Equal("Building002", Building[0].Name);
            Assert.Equal(1, Building[0].FacilityId);
        }

        #endregion

        #region Get By FacilityId  

        [Fact]
        public async void GetBuildingByFacilityId_ValidData_OkResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var Facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();

            //Act  
            var data = await controller.GetBuildingsByFacility(Facility.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetBuildingByFacilityId_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var FacilityId = 10;

            //Act  
            var data = await controller.GetBuildingsByFacility(FacilityId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetBuildingByFacilityId_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            int? FacilityId = null;

            //Act  
            var data = await controller.GetBuildingsByFacility(FacilityId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetBuildingByFacilityId_MatchResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var Facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();

            //Act  
            var data = await controller.GetBuildingsByFacility(Facility.Id);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Building = okResult.Value.Should().BeAssignableTo<List<Building>>().Subject;

            Assert.Equal("Building001", Building[0].Name);
        }

        #endregion


        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var buildingId = dbContext.Buildings.Where(x => x.Name == "Building001").FirstOrDefault().Id;

            var Building = new BuildingRequest();
            Building.Name = "Building001";
            Building.FacilityId = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateBuilding(buildingId, Building);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var buildingId = dbContext.Buildings.Where(x => x.Name == "Building001").FirstOrDefault().Id;
            var Building = new BuildingRequest();
            Building.Name = null;
            Building.FacilityId = 0;

            //Act  
            var data = await controller.UpdateBuilding(buildingId, Building);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new BuildingController(_repository);

            var buildingId = 5;
            var Building = new BuildingRequest();
            Building.Name = "Building005";
            Building.FacilityId = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault().Id;

            //Act                    
            var data = await controller.UpdateBuilding(buildingId, Building);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequest()
        {
            //Arrange  
            var controller = new BuildingController(_repository);

            var buildingId = 0;
            var Building = new BuildingRequest();
            Building.Name = "Building005";
            Building.FacilityId = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault().Id;

            //Act                    
            var data = await controller.UpdateBuilding(buildingId, Building);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion



        #region Delete Building  

        [Fact]
        public async void Delete_Building_Return_OkResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var BuildingId = 2;

            //Act  
            var data = await controller.DeleteBuilding(BuildingId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_Building_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            var BuildingId = 5;

            //Act  
            var data = await controller.DeleteBuilding(BuildingId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new BuildingController(_repository);
            int? BuildingId = null;

            //Act  
            var data = await controller.DeleteBuilding(BuildingId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

