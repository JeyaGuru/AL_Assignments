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
    public class WaterMeterControllerTests
    {
        private WaterMeterRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public WaterMeterControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb_WaterMeter")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new WaterMeterRepository(dbContext);

        }
        [Fact]
        public async void AddWaterMeter_ValidData_ReturnOkResult()
        {
            //arrange
            var waterMeterController = new WaterMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone001").FirstOrDefault();
            var waterMeter = new WaterMeterRequest() { Number = "WaterMeter_001", ZoneId = Zone.Id };

            //act
            var data = await waterMeterController.AddWaterMeter(waterMeter);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddWaterMeter_InValidData_ReturnBadRequest()
        {
            //arrange
            var waterMeterController = new WaterMeterController(_repository);
            WaterMeterRequest waterMeter = null;

            //act
            var data = await waterMeterController.AddWaterMeter(waterMeter);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddWaterMeter_InValidData_Return_BadRequest()
        {
            //arrange
            var waterMeterController = new WaterMeterController(_repository);
            var waterMeter = new WaterMeterRequest() { Number = null, ZoneId = 0 };

            //act
            var data = await waterMeterController.AddWaterMeter(waterMeter);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddWaterMeter_ValidData_MatchResult()
        {
            //Arrange  
            var waterMeterController = new WaterMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault();
            var waterMeter = new WaterMeterRequest() { Number = "WaterMeter_002", ZoneId = Zone.Id };


            //Act  
            var data = await waterMeterController.AddWaterMeter(waterMeter);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await waterMeterController.GetWaterMeter((int?)okResult.Value);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeter = okResult2.Value.Should().BeAssignableTo<WaterMeter>().Subject;

            Assert.Equal("WaterMeter_002", WaterMeter.Number);
            Assert.Equal(Zone.Id, WaterMeter.ZoneId);
        }

        [Fact]
        public async void AddWaterMeter_ExistingData_ReturnOkResult_ShouldNotBeAdded()
        {
            //arrange
            var waterMeterController = new WaterMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone001").FirstOrDefault();
            var waterMeter = new WaterMeterRequest() { Number = "WaterMeter001", ZoneId = Zone.Id };
            var countBeforeAddition = await dbContext.WaterMeters.CountAsync();

            //act
            var data = await waterMeterController.AddWaterMeter(waterMeter);
            var countAfterAddition = await dbContext.WaterMeters.CountAsync();

            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(countBeforeAddition, countAfterAddition);
        }

        #region Get By Id  

        [Fact]
        public async void GetWaterMeterById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            var WaterMeterId = 1;

            //Act  
            var data = await controller.GetWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetWaterMeterById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            var WaterMeterId = 10;

            //Act  
            var data = await controller.GetWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetWaterMeterById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            int? WaterMeterId = null;

            //Act  
            var data = await controller.GetWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetWaterMeterById_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            int? WaterMeterId = 1;

            //Act  
            var data = await controller.GetWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeter = okResult.Value.Should().BeAssignableTo<WaterMeter>().Subject;

            Assert.Equal("WaterMeter001", WaterMeter.Number);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetWaterMeters_Return_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);

            //Act  
            var data = await controller.GetWaterMeters();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetWaterMeters_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);

            //Act  
            var data = await controller.GetWaterMeters();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeter = okResult.Value.Should().BeAssignableTo<List<WaterMeter>>().Subject;

            Assert.Equal("WaterMeter004", WaterMeter[0].Number);
            Assert.Equal(4, WaterMeter[0].ZoneId);
        }

        #endregion

        #region Get By ZoneId  

        [Fact]
        public async void GetWaterMeterByZoneId_ValidData_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault();

            //Act  
            var data = await controller.GetWaterMetersByZone(Zone.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetWaterMeterByZoneId_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            var ZoneId = 10;

            //Act  
            var data = await controller.GetWaterMetersByZone(ZoneId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetWaterMeterByZoneId_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            int? ZoneId = null;

            //Act  
            var data = await controller.GetWaterMetersByZone(ZoneId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetWaterMeterByZoneId_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault();

            //Act  
            var data = await controller.GetWaterMetersByZone(Zone.Id);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeter = okResult.Value.Should().BeAssignableTo<List<WaterMeter>>().Subject;

            Assert.Equal("WaterMeter002", WaterMeter[0].Number);
        }

        #endregion


        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);

            var WaterMeterId = 1;
            var WaterMeter = new WaterMeterRequest();

            WaterMeter.Number = "WaterMeter001";
            WaterMeter.ZoneId = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateWaterMeter(WaterMeterId, WaterMeter);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            var WaterMeterId = 1;
            var WaterMeter = new WaterMeterRequest();

            WaterMeter.Number = null;
            WaterMeter.ZoneId = 0;
            //Act              
            var updatedData = await controller.UpdateWaterMeter(WaterMeterId, WaterMeter);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);

            var WaterMeterId = 10;
            var WaterMeter = new WaterMeterRequest();

            WaterMeter.Number = "WaterMeter001";
            WaterMeter.ZoneId = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateWaterMeter(WaterMeterId, WaterMeter);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequest()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);

            var WaterMeterId = 0;
            var WaterMeter = new WaterMeterRequest();

            WaterMeter.Number = "WaterMeter001";
            WaterMeter.ZoneId = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateWaterMeter(WaterMeterId, WaterMeter);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        #endregion



        #region Delete WaterMeter  

        [Fact]
        public async void Delete_WaterMeter_Return_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            var WaterMeterId = 2;

            //Act  
            var data = await controller.DeleteWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_WaterMeter_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            var WaterMeterId = 5;

            //Act  
            var data = await controller.DeleteWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new WaterMeterController(_repository);
            int? WaterMeterId = null;

            //Act  
            var data = await controller.DeleteWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

