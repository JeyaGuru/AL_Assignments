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
    public class ZoneControllerTests
    {
        private ZoneRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public ZoneControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb_Zone")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new ZoneRepository(dbContext);

        }
        [Fact]
        public async void AddZone_ValidData_ReturnOkResult()
        {
            //arrange
            var zoneController = new ZoneController(_repository);
            var Floor = dbContext.Floors.Where(x => x.Name == "Floor001").FirstOrDefault();
            var zone = new ZoneRequest() { Name = "Zone_001", FloorId = Floor.Id };

            //act
            var data = await zoneController.AddZone(zone);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddZone_InValidData_ReturnBadRequest()
        {
            //arrange
            var zoneController = new ZoneController(_repository);
            ZoneRequest zone = null;

            //act
            var data = await zoneController.AddZone(zone);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddZone_InValidData_Return_BadRequest()
        {
            //arrange
            var zoneController = new ZoneController(_repository);
            var zone = new ZoneRequest() { Name = null, FloorId = 0 };

            //act
            var data = await zoneController.AddZone(zone);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddZone_ValidData_MatchResult()
        {
            //Arrange  
            var zoneController = new ZoneController(_repository);
            var Floor = dbContext.Floors.Where(x => x.Name == "Floor002").FirstOrDefault();
            var zone = new ZoneRequest() { Name = "Zone_002", FloorId = Floor.Id };


            //Act  
            var data = await zoneController.AddZone(zone);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await zoneController.GetZone((int?)okResult.Value);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var Zone = okResult2.Value.Should().BeAssignableTo<Zone>().Subject;

            Assert.Equal("Zone_002", Zone.Name);
            Assert.Equal(Floor.Id, Zone.FloorId);
        }

        [Fact]
        public async void AddZone_ExistingData_ReturnOkResult_ShouldNotBeAdded()
        {
            //arrange
            var zoneController = new ZoneController(_repository);
            var Floor = dbContext.Floors.Where(x => x.Name == "Floor001").FirstOrDefault();
            var zone = new ZoneRequest() { Name = "Zone001", FloorId = Floor.Id };
            var countBeforeAddition = await dbContext.Zones.CountAsync();

            //act
            var data = await zoneController.AddZone(zone);
            var countAfterAddition = await dbContext.Zones.CountAsync();

            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(countBeforeAddition, countAfterAddition);
        }

        #region Get By Id  

        [Fact]
        public async void GetZoneById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var ZoneId = 1;

            //Act  
            var data = await controller.GetZone(ZoneId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetZoneById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var ZoneId = 10;

            //Act  
            var data = await controller.GetZone(ZoneId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetZoneById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            int? ZoneId = null;

            //Act  
            var data = await controller.GetZone(ZoneId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetZoneById_MatchResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            int? ZoneId = 1;

            //Act  
            var data = await controller.GetZone(ZoneId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Zone = okResult.Value.Should().BeAssignableTo<Zone>().Subject;

            Assert.Equal("Zone001", Zone.Name);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetZones_Return_OkResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);

            //Act  
            var data = await controller.GetZones();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetZones_MatchResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);

            //Act  
            var data = await controller.GetZones();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Zone = okResult.Value.Should().BeAssignableTo<List<Zone>>().Subject;

            Assert.Equal("Zone003", Zone[0].Name);
            Assert.Equal(2, Zone[0].FloorId);
        }

        #endregion

        #region Get By FloorId  

        [Fact]
        public async void GetZoneByFloorId_ValidData_OkResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var Floor = dbContext.Floors.Where(x => x.Name == "Floor002").FirstOrDefault();

            //Act  
            var data = await controller.GetZonesByFloor(Floor.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetZoneByFloorId_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var FloorId = 10;

            //Act  
            var data = await controller.GetZonesByFloor(FloorId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetZoneByFloorId_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            int? FloorId = null;

            //Act  
            var data = await controller.GetZonesByFloor(FloorId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetZoneByFloorId_MatchResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var Floor = dbContext.Floors.Where(x => x.Name == "Floor002").FirstOrDefault();

            //Act  
            var data = await controller.GetZonesByFloor(Floor.Id);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var Zone = okResult.Value.Should().BeAssignableTo<List<Zone>>().Subject;

            Assert.Equal("Zone003", Zone[0].Name);
        }

        #endregion


        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);

            var ZoneId = 1;
            var Zone = new ZoneRequest();

            Zone.Name = "Zone001";
            Zone.FloorId = dbContext.Floors.Where(x => x.Name == "Floor002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateZone(ZoneId, Zone);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var ZoneId = 1;
            var Zone = new ZoneRequest();

            Zone.Name = null;
            Zone.FloorId = 0;
            //Act              
            var updatedData = await controller.UpdateZone(ZoneId, Zone);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new ZoneController(_repository);

            var ZoneId = 10;
            var Zone = new ZoneRequest();

            Zone.Name = "Zone001";
            Zone.FloorId = dbContext.Floors.Where(x => x.Name == "Floor002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateZone(ZoneId, Zone);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequest()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var ZoneId = 0;
            var Zone = new ZoneRequest();

            Zone.Name = "Zone001";
            Zone.FloorId = dbContext.Floors.Where(x => x.Name == "Floor002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateZone(ZoneId, Zone);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        #endregion



        #region Delete Zone  

        [Fact]
        public async void Delete_Zone_Return_OkResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var ZoneId = 2;

            //Act  
            var data = await controller.DeleteZone(ZoneId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_Zone_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            var ZoneId = 5;

            //Act  
            var data = await controller.DeleteZone(ZoneId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new ZoneController(_repository);
            int? ZoneId = null;

            //Act  
            var data = await controller.DeleteZone(ZoneId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

