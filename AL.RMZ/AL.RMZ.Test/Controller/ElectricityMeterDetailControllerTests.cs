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
    public class ElectricityMeterDetailControllerTests
    {
        private ElectricityMeterDetailRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public ElectricityMeterDetailControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb_ElectricityMeterDetail")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new ElectricityMeterDetailRepository(dbContext);

        }
        [Fact]
        public async void AddElectricityMeterDetail_ValidData_ReturnOkResult()
        {
            //arrange
            var electricityMeterDetailController = new ElectricityMeterDetailController(_repository);
            var ElectricityMeter = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter001").FirstOrDefault();
            var electricityMeterDetail = new ElectricityMeterDetailRequest() { ReadingDate = DateTime.Now.AddDays(-30), ElectricityMeterId = ElectricityMeter.Id, StartReading = 0, EndReading = 100 };

            //act
            var data = await electricityMeterDetailController.AddElectricityMeterDetail(electricityMeterDetail);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddElectricityMeterDetail_InValidData_ReturnBadRequest()
        {
            //arrange
            var electricityMeterDetailController = new ElectricityMeterDetailController(_repository);
            ElectricityMeterDetailRequest electricityMeterDetail = null;

            //act
            var data = await electricityMeterDetailController.AddElectricityMeterDetail(electricityMeterDetail);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddElectricityMeterDetail_InValidData_Return_BadRequest()
        {
            //arrange
            var electricityMeterDetailController = new ElectricityMeterDetailController(_repository);
            var electricityMeterDetail = new ElectricityMeterDetailRequest() { ReadingDate = default, ElectricityMeterId = 0, StartReading = 0, EndReading = 0 };

            //act
            var data = await electricityMeterDetailController.AddElectricityMeterDetail(electricityMeterDetail);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddElectricityMeterDetail_ValidData_MatchResult()
        {
            //Arrange  
            var electricityMeterDetailController = new ElectricityMeterDetailController(_repository);
            var ElectricityMeter = dbContext.ElectricityMeters.Where(x => x.Id == 4).FirstOrDefault();
            var electricityMeterDetail = new ElectricityMeterDetailRequest() { ReadingDate = DateTime.Now, ElectricityMeterId = ElectricityMeter.Id, StartReading = 100, EndReading = 200 };


            //Act  
            var data = await electricityMeterDetailController.AddElectricityMeterDetail(electricityMeterDetail);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await electricityMeterDetailController.GetElectricityMeterDetail((int?)okResult.Value);

            //Assert          
            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeterDetail = okResult2.Value.Should().BeAssignableTo<ElectricityMeterDetail>().Subject;

            Assert.Equal(DateTime.Now.ToShortDateString(), ElectricityMeterDetail.ReadingDate.ToShortDateString());
            Assert.Equal(ElectricityMeter.Id, ElectricityMeterDetail.ElectricityMeterId);
            Assert.Equal(100, ElectricityMeterDetail.StartReading);
            Assert.Equal(200, ElectricityMeterDetail.EndReading);
            Assert.Equal(100, ElectricityMeterDetail.TotalUnits);
        }
        [Fact]
        public async void AddElectricityMeterDetail_ExistngData_ReturnOkResult_ShouldNotBeAdded()
        {
            //arrange
            var electricityMeterDetailController = new ElectricityMeterDetailController(_repository);
            var ElectricityMeter = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter001").FirstOrDefault();
            var electricityMeterDetail = new ElectricityMeterDetailRequest() { ReadingDate = DateTime.Now.AddDays(-30), ElectricityMeterId = ElectricityMeter.Id, StartReading = 0, EndReading = 100 };
            var countBeforeAddition = await dbContext.ElectricityMeterDetails.CountAsync();

            //act
            var data = await electricityMeterDetailController.AddElectricityMeterDetail(electricityMeterDetail);
            var countAfterAddition = await dbContext.ElectricityMeterDetails.CountAsync();

            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(countBeforeAddition, countAfterAddition);
        }

        #region Get By Id  

        [Fact]
        public async void GetElectricityMeterDetailById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeterDetailId = 1;

            //Act  
            var data = await controller.GetElectricityMeterDetail(ElectricityMeterDetailId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterDetailById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeterDetailId = 10;

            //Act  
            var data = await controller.GetElectricityMeterDetail(ElectricityMeterDetailId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterDetailById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            int? ElectricityMeterDetailId = null;

            //Act  
            var data = await controller.GetElectricityMeterDetail(ElectricityMeterDetailId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterDetailById_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            int? ElectricityMeterDetailId = 1;

            //Act  
            var data = await controller.GetElectricityMeterDetail(ElectricityMeterDetailId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeterDetail = okResult.Value.Should().BeAssignableTo<ElectricityMeterDetail>().Subject;

            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), ElectricityMeterDetail.ReadingDate.ToShortDateString());
            Assert.Equal(1, ElectricityMeterDetail.ElectricityMeterId);
            Assert.Equal(0, ElectricityMeterDetail.StartReading);
            Assert.Equal(100, ElectricityMeterDetail.EndReading);
            Assert.Equal(100, ElectricityMeterDetail.TotalUnits);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetElectricityMeterDetails_Return_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);

            //Act  
            var data = await controller.GetElectricityMeterDetails();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetElectricityMeterDetails_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);

            //Act  
            var data = await controller.GetElectricityMeterDetails();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeterDetail = okResult.Value.Should().BeAssignableTo<List<ElectricityMeterDetail>>().Subject;

            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), ElectricityMeterDetail[0].ReadingDate.ToShortDateString());
            Assert.Equal(4, ElectricityMeterDetail[0].ElectricityMeterId);
            Assert.Equal(10, ElectricityMeterDetail[0].StartReading);
            Assert.Equal(100, ElectricityMeterDetail[0].EndReading);
            Assert.Equal(90, ElectricityMeterDetail[0].TotalUnits);
        }

        #endregion

        #region Get By ElectricityMeterId  

        [Fact]
        public async void GetElectricityMeterDetailByElectricityMeterId_ValidData_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeter = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter002").FirstOrDefault();

            //Act  
            var data = await controller.GetElectricityMeterDetailsByElectricityMeter(ElectricityMeter.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterDetailByElectricityMeterId_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeterId = 10;

            //Act  
            var data = await controller.GetElectricityMeterDetailsByElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterDetailByElectricityMeterId_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            int? ElectricityMeterId = null;

            //Act  
            var data = await controller.GetElectricityMeterDetailsByElectricityMeter(ElectricityMeterId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetElectricityMeterDetailByElectricityMeterId_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeter = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter002").FirstOrDefault();

            //Act  
            var data = await controller.GetElectricityMeterDetailsByElectricityMeter(ElectricityMeter.Id);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeterDetail = okResult.Value.Should().BeAssignableTo<List<ElectricityMeterDetail>>().Subject;

            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), ElectricityMeterDetail[0].ReadingDate.ToShortDateString());
            Assert.Equal(ElectricityMeter.Id, ElectricityMeterDetail[0].ElectricityMeterId);
            Assert.Equal(10, ElectricityMeterDetail[0].StartReading);
            Assert.Equal(100, ElectricityMeterDetail[0].EndReading);
            Assert.Equal(90, ElectricityMeterDetail[0].TotalUnits);
        }

        #endregion

        #region ElectricityMeterDetail By different Param
        [Fact]
        public async void GetElectricityMeterDetailByParams_FacilityId_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var Facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();

            //Act  
            var data = await controller.GetElectricityMeterDetailByParams(Facility.Id, null, null, null, null, null, null);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeterDetail = okResult.Value.Should().BeAssignableTo<List<DisplayElectricityMeterDetail>>().Subject;

            Assert.Equal("Carmelram", ElectricityMeterDetail[0].facilityname);
            Assert.Equal("Building001", ElectricityMeterDetail[0].buildingname);
            Assert.Equal("Zone002", ElectricityMeterDetail[0].zonename);
            Assert.Equal(10, ElectricityMeterDetail[0].startunit);
            Assert.Equal(100, ElectricityMeterDetail[0].endunit);
            Assert.Equal(90, ElectricityMeterDetail[0].totalunits);
            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), ElectricityMeterDetail[0].readingdate.ToShortDateString());
            Assert.Equal("ElectricityMeter002", ElectricityMeterDetail[0].electricitymeter);
        }

        [Fact]
        public async void GetElectricityMeterDetailByParams_BuildingId_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();
            var building = dbContext.Buildings.Where(x => x.Name == "Building001" && x.FacilityId == facility.Id).FirstOrDefault();

            //Act  
            var data = await controller.GetElectricityMeterDetailByParams(null, building.Id, null, null, null, null, null);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeterDetail = okResult.Value.Should().BeAssignableTo<List<DisplayElectricityMeterDetail>>().Subject;

            Assert.Equal("Carmelram", ElectricityMeterDetail[0].facilityname);
            Assert.Equal("Building001", ElectricityMeterDetail[0].buildingname);
            Assert.Equal("Zone002", ElectricityMeterDetail[0].zonename);
            Assert.Equal(10, ElectricityMeterDetail[0].startunit);
            Assert.Equal(100, ElectricityMeterDetail[0].endunit);
            Assert.Equal(90, ElectricityMeterDetail[0].totalunits);
            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), ElectricityMeterDetail[0].readingdate.ToShortDateString());
            Assert.Equal("ElectricityMeter002", ElectricityMeterDetail[0].electricitymeter);
        }

        [Fact]
        public async void GetElectricityMeterDetailByParams_ZoneId_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();
            var building = dbContext.Buildings.Where(x => x.Name == "Building001" && x.FacilityId == facility.Id).FirstOrDefault();
            var floor = dbContext.Floors.Where(x => x.Name == "Floor001" && x.BuildingId == building.Id).FirstOrDefault();
            var zone = dbContext.Zones.Where(x => x.Name == "Zone002" && x.FloorId == floor.Id).FirstOrDefault();
            //Act  
            var data = await controller.GetElectricityMeterDetailByParams(null, null, null, zone.Id, null, null, null);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeterDetail = okResult.Value.Should().BeAssignableTo<List<DisplayElectricityMeterDetail>>().Subject;

            Assert.Equal("Carmelram", ElectricityMeterDetail[0].facilityname);
            Assert.Equal("Building001", ElectricityMeterDetail[0].buildingname);
            Assert.Equal("Zone002", ElectricityMeterDetail[0].zonename);
            Assert.Equal(10, ElectricityMeterDetail[0].startunit);
            Assert.Equal(100, ElectricityMeterDetail[0].endunit);
            Assert.Equal(90, ElectricityMeterDetail[0].totalunits);
            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), ElectricityMeterDetail[0].readingdate.ToShortDateString());
            Assert.Equal("ElectricityMeter002", ElectricityMeterDetail[0].electricitymeter);
        }

        [Fact]
        public async void GetElectricityMeterDetailByParams_DateRange_MatchResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var startDate = DateTime.Now.AddDays(-30);
            var endDate = DateTime.Now;
            //Act  
            var data = await controller.GetElectricityMeterDetailByParams(null, null, null, null, null, startDate, endDate);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var ElectricityMeterDetail = okResult.Value.Should().BeAssignableTo<List<DisplayElectricityMeterDetail>>().Subject;

            Assert.Equal(4, ElectricityMeterDetail.Count());
            Assert.Equal("Carmelram", ElectricityMeterDetail[0].facilityname);
            Assert.Equal("Building001", ElectricityMeterDetail[0].buildingname);
            Assert.Equal("Zone002", ElectricityMeterDetail[0].zonename);
            Assert.Equal(10, ElectricityMeterDetail[0].startunit);
            Assert.Equal(100, ElectricityMeterDetail[0].endunit);
            Assert.Equal(90, ElectricityMeterDetail[0].totalunits);
            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), ElectricityMeterDetail[0].readingdate.ToShortDateString());
            Assert.Equal("ElectricityMeter002", ElectricityMeterDetail[0].electricitymeter);
        }

        [Fact]
        public async void GetElectricityMeterDetailByParams_OkObject()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);

            //Act  
            var data = await controller.GetElectricityMeterDetailByParams(null, null, null, null, null, null, null);

            //Assert             
            Assert.IsType<OkObjectResult>(data);
        }
        #endregion

        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);

            var ElectricityMeterDetailId = 3;

            var ElectricityMeterDetail = new ElectricityMeterDetailRequest();

            ElectricityMeterDetail.ReadingDate = DateTime.Now;
            ElectricityMeterDetail.StartReading = 10;
            ElectricityMeterDetail.EndReading = 250;
            ElectricityMeterDetail.ElectricityMeterId = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateElectricityMeterDetail(ElectricityMeterDetailId, ElectricityMeterDetail);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);

            var ElectricityMeterDetailId = 1;

            var ElectricityMeterDetail = new ElectricityMeterDetailRequest();
            ElectricityMeterDetail.ReadingDate = default;
            ElectricityMeterDetail.ElectricityMeterId = 0;
            ElectricityMeterDetail.StartReading = 0;
            ElectricityMeterDetail.EndReading = 0;

            //Act  
            var updatedData = await controller.UpdateElectricityMeterDetail(ElectricityMeterDetailId, ElectricityMeterDetail);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeterDetailId = 10;

            var ElectricityMeterDetail = new ElectricityMeterDetailRequest();
            ElectricityMeterDetail.StartReading = 100;
            ElectricityMeterDetail.EndReading = 500;
            ElectricityMeterDetail.ReadingDate = DateTime.Now;
            ElectricityMeterDetail.ElectricityMeterId = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter002").FirstOrDefault().Id;

            //Act                    
            var updatedData = await controller.UpdateElectricityMeterDetail(ElectricityMeterDetailId, ElectricityMeterDetail);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequest()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeterDetailId = 0;

            var ElectricityMeterDetail = new ElectricityMeterDetailRequest();
            ElectricityMeterDetail.StartReading = 100;
            ElectricityMeterDetail.EndReading = 500;
            ElectricityMeterDetail.ReadingDate = DateTime.Now;
            ElectricityMeterDetail.ElectricityMeterId = dbContext.ElectricityMeters.Where(x => x.Number == "ElectricityMeter002").FirstOrDefault().Id;

            //Act                    
            var updatedData = await controller.UpdateElectricityMeterDetail(ElectricityMeterDetailId, ElectricityMeterDetail);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        #endregion



        #region Delete ElectricityMeterDetail  

        [Fact]
        public async void Delete_ElectricityMeterDetail_Return_OkResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeterDetailId = 2;

            //Act  
            var data = await controller.DeleteElectricityMeterDetail(ElectricityMeterDetailId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_ElectricityMeterDetail_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            var ElectricityMeterDetailId = 5;

            //Act  
            var data = await controller.DeleteElectricityMeterDetail(ElectricityMeterDetailId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Null_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new ElectricityMeterDetailController(_repository);
            int? ElectricityMeterDetailId = null;

            //Act  
            var data = await controller.DeleteElectricityMeterDetail(ElectricityMeterDetailId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

