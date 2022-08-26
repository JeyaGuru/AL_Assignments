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
    public class FloorControllerTests
    {
        private FloorRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public FloorControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb_Floor")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new FloorRepository(dbContext);

        }
        [Fact]
        public async void AddFloor_ValidData_ReturnOkResult()
        {
            //arrange
            var floorController = new FloorController(_repository);
            var Building = dbContext.Buildings.Where(x => x.Name == "Building001").FirstOrDefault();
            var floor = new FloorRequest() { Name = "Floor_001", BuildingId = Building.Id };

            //act
            var data = await floorController.AddFloor(floor);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddFloor_InValidData_ReturnBadRequest()
        {
            //arrange
            var floorController = new FloorController(_repository);
            FloorRequest floor = null;

            //act
            var data = await floorController.AddFloor(floor);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddFloor_InValidData_Return_BadRequest()
        {
            //arrange
            var floorController = new FloorController(_repository);
            var floor = new FloorRequest() { Name = null, BuildingId = 0 };

            //act
            var data = await floorController.AddFloor(floor);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddFloor_ValidData_MatchResult()
        {
            //Arrange  
            var floorController = new FloorController(_repository);
            var Building = dbContext.Buildings.Where(x => x.Name == "Building002").FirstOrDefault();
            var floor = new FloorRequest() { Name = "Floor_002", BuildingId = Building.Id };


            //Act  
            var data = await floorController.AddFloor(floor);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await floorController.GetFloor((int?)okResult.Value);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var Floor = okResult2.Value.Should().BeAssignableTo<Floor>().Subject;

            Assert.Equal("Floor_002", Floor.Name);
            Assert.Equal(Building.Id, Floor.BuildingId);
        }

        [Fact]
        public async void AddFloor_ExistingData_ReturnOkResult_ShouldNotBeAdded()
        {
            //arrange
            var floorController = new FloorController(_repository);
            var Building = dbContext.Buildings.Where(x => x.Name == "Building001").FirstOrDefault();
            var floor = new FloorRequest() { Name = "Floor001", BuildingId = Building.Id };
            var countBeforeAddition = await dbContext.Floors.CountAsync();

            //act
            var data = await floorController.AddFloor(floor);
            var countAfterAddition = await dbContext.Floors.CountAsync();

            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(countBeforeAddition, countAfterAddition);
        }
        #region Get By Id  

        [Fact]
        public async void GetFloorById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var FloorId = 1;

            //Act  
            var data = await controller.GetFloor(FloorId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetFloorById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var FloorId = 10;

            //Act  
            var data = await controller.GetFloor(FloorId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetFloorById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            int? FloorId = null;

            //Act  
            var data = await controller.GetFloor(FloorId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetFloorById_MatchResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            int? FloorId = 1;

            //Act  
            var data = await controller.GetFloor(FloorId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Floor = okResult.Value.Should().BeAssignableTo<Floor>().Subject;

            Assert.Equal("Floor001", Floor.Name);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetFloors_Return_OkResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);

            //Act  
            var data = await controller.GetFloors();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetFloors_MatchResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);

            //Act  
            var data = await controller.GetFloors();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Floor = okResult.Value.Should().BeAssignableTo<List<Floor>>().Subject;

            Assert.Equal("Floor002", Floor[0].Name);
            Assert.Equal(2, Floor[0].BuildingId);
        }

        #endregion

        #region Get By BuildingId  

        [Fact]
        public async void GetFloorByBuildingId_ValidData_OkResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var Building = dbContext.Buildings.Where(x => x.Name == "Building002").FirstOrDefault();

            //Act  
            var data = await controller.GetFloorsByBuilding(Building.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetFloorByBuildingId_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var BuildingId = 10;

            //Act  
            var data = await controller.GetFloorsByBuilding(BuildingId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetFloorByBuildingId_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            int? BuildingId = null;

            //Act  
            var data = await controller.GetFloorsByBuilding(BuildingId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetFloorByBuildingId_MatchResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var Building = dbContext.Buildings.Where(x => x.Name == "Building002").FirstOrDefault();

            //Act  
            var data = await controller.GetFloorsByBuilding(Building.Id);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Floor = okResult.Value.Should().BeAssignableTo<List<Floor>>().Subject;

            Assert.Equal("Floor002", Floor[0].Name);
        }

        #endregion


        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var floorId = 1;
            var Floor = new FloorRequest();

            Floor.Name = "Floor001";
            Floor.BuildingId = dbContext.Buildings.Where(x => x.Name == "Building002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateFloor(floorId, Floor);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var floorId = 1;
            var Floor = new FloorRequest();

            Floor.Name = null;
            Floor.BuildingId = 0;
            //Act              
            var updatedData = await controller.UpdateFloor(floorId, Floor);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var floorId = 10;
            var Floor = new FloorRequest();

            Floor.Name = "Floor001";
            Floor.BuildingId = dbContext.Buildings.Where(x => x.Name == "Building002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateFloor(floorId, Floor);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(updatedData);
        }
        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequest()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var floorId = 0;
            var Floor = new FloorRequest();

            Floor.Name = "Floor001";
            Floor.BuildingId = dbContext.Buildings.Where(x => x.Name == "Building002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateFloor(floorId, Floor);

            //Assert  
            Assert.IsType<BadRequestResult>(updatedData);
        }

        #endregion



        #region Delete Floor  

        [Fact]
        public async void Delete_Floor_Return_OkResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var FloorId = 2;

            //Act  
            var data = await controller.DeleteFloor(FloorId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_Floor_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            var FloorId = 5;

            //Act  
            var data = await controller.DeleteFloor(FloorId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new FloorController(_repository);
            int? FloorId = null;

            //Act  
            var data = await controller.DeleteFloor(FloorId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

