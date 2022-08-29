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
    public class ElectricityMeterControllerTests
    {
        private ElectricityMeterRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public ElectricityMeterControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb_ElectricityMeter")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new ElectricityMeterRepository(dbContext);

        }
        [Fact]
        public async void AddElectricityMeter_ValidData_ReturnOkResult()
        {
            //arrange
            var electricityMeterController = new ElectricityMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone001").FirstOrDefault();
            var electricityMeter = new ElectricityMeterRequest() { Number = "ElectricityMeter_001", ZoneId = Zone.Id };

            //act
            var data = await electricityMeterController.AddElectricityMeter(electricityMeter);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddElectricityMeter_InValidData_ReturnBadRequest()
        {
            //arrange
            var electricityMeterController = new ElectricityMeterController(_repository);
            ElectricityMeterRequest electricityMeter = null;

            //act
            var data = await electricityMeterController.AddElectricityMeter(electricityMeter);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddElectricityMeter_InValidData_Return_BadRequest()
        {
            //arrange
            var electricityMeterController = new ElectricityMeterController(_repository);
            var electricityMeter = new ElectricityMeterRequest() { Number = null, ZoneId = 0 };

            //act
            var data = await electricityMeterController.AddElectricityMeter(electricityMeter);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddElectricityMeter_ValidData_MatchResult()
        {
            //Arrange  
            var electricityMeterController = new ElectricityMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault();
            var electricityMeter = new ElectricityMeterRequest() { Number = "ElectricityMeter_002", ZoneId = Zone.Id };


            //Act  
            var data = await electricityMeterController.AddElectricityMeter(electricityMeter);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await electricityMeterController.GetElectricityMeter((int?)okResult.Value);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeter = okResult2.Value.Should().BeAssignableTo<ElectricityMeter>().Subject;

            Assert.Equal("ElectricityMeter_002", ElectricityMeter.Number);
            Assert.Equal(Zone.Id, ElectricityMeter.ZoneId);
        }

        [Fact]
        public async void AddElectricityMeter_ExistingData_ReturnOkResult_ShouldNotBeAdded()
        {
            //arrange
            var electricityMeterController = new ElectricityMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone001").FirstOrDefault();
            var electricityMeter = new ElectricityMeterRequest() { Number = "ElectricityMeter001", ZoneId = Zone.Id };
            var countBeforeAddition = await dbContext.ElectricityMeters.CountAsync();

            //act
            var data = await electricityMeterController.AddElectricityMeter(electricityMeter);
            var countAfterAddition = await dbContext.ElectricityMeters.CountAsync();

            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(countBeforeAddition, countAfterAddition);
        }

        #region Get By Id  

        [Fact]
        public async void GetElectricityMeterById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            var ElectricityMeterId = 1;

            //Act  
            var data = await controller.GetElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            var ElectricityMeterId = 10;

            //Act  
            var data = await controller.GetElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            int? ElectricityMeterId = null;

            //Act  
            var data = await controller.GetElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterById_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            int? ElectricityMeterId = 1;

            //Act  
            var data = await controller.GetElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeter = okResult.Value.Should().BeAssignableTo<ElectricityMeter>().Subject;

            Assert.Equal("ElectricityMeter001", ElectricityMeter.Number);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetElectricityMeters_Return_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);

            //Act  
            var data = await controller.GetElectricityMeters();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetElectricityMeters_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);

            //Act  
            var data = await controller.GetElectricityMeters();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeter = okResult.Value.Should().BeAssignableTo<List<ElectricityMeter>>().Subject;

            Assert.Equal("ElectricityMeter004", ElectricityMeter[0].Number);
            Assert.Equal(4, ElectricityMeter[0].ZoneId);
        }

        #endregion

        #region Get By ZoneId  

        [Fact]
        public async void GetElectricityMeterByZoneId_ValidData_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault();

            //Act  
            var data = await controller.GetElectricityMetersByZone(Zone.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterByZoneId_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            var ZoneId = 10;

            //Act  
            var data = await controller.GetElectricityMetersByZone(ZoneId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterByZoneId_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            int? ZoneId = null;

            //Act  
            var data = await controller.GetElectricityMetersByZone(ZoneId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterByZoneId_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            var Zone = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault();

            //Act  
            var data = await controller.GetElectricityMetersByZone(Zone.Id);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeter = okResult.Value.Should().BeAssignableTo<List<ElectricityMeter>>().Subject;

            Assert.Equal("ElectricityMeter002", ElectricityMeter[0].Number);
        }

        #endregion


        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);

            var electrcityMeterId = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter001").FirstOrDefault().Id;
            var ElectricityMeter = new ElectricityMeterRequest();
            ElectricityMeter.Number = "ElectricityMeter001";
            ElectricityMeter.ZoneId = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateElectricityMeter(electrcityMeterId, ElectricityMeter);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            var electrcityMeterId = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter001").FirstOrDefault().Id;

            var ElectricityMeter = new ElectricityMeterRequest();
            ElectricityMeter.Number = null;
            ElectricityMeter.ZoneId = 0;
            //Act              
            var updatedData = await controller.UpdateElectricityMeter(electrcityMeterId, ElectricityMeter);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);

            var electrcityMeterId = 10;
            var ElectricityMeter = new ElectricityMeterRequest();
            ElectricityMeter.Number = "ElectricityMeter001";
            ElectricityMeter.ZoneId = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateElectricityMeter(electrcityMeterId, ElectricityMeter);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequest()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);

            var electrcityMeterId = 0;
            var ElectricityMeter = new ElectricityMeterRequest();
            ElectricityMeter.Number = "ElectricityMeter001";
            ElectricityMeter.ZoneId = dbContext.Zones.Where(x => x.Name == "Zone002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateElectricityMeter(electrcityMeterId, ElectricityMeter);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        #endregion



        #region Delete ElectricityMeter  

        [Fact]
        public async void Delete_ElectricityMeter_Return_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            var ElectricityMeterId = 2;

            //Act  
            var data = await controller.DeleteElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_ElectricityMeter_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            var ElectricityMeterId = 5;

            //Act  
            var data = await controller.DeleteElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new ElectricityMeterController(_repository);
            int? ElectricityMeterId = null;

            //Act  
            var data = await controller.DeleteElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

