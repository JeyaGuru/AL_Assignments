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
    public class WaterMeterDetailControllerTests
    {
        private WaterMeterDetailRepository _repository;
        private DbContextOptions<RMZAPIDbContext> _dbContextOptions { get; }
        RMZAPIDbContext dbContext;
        public WaterMeterDetailControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<RMZAPIDbContext>()
                .UseInMemoryDatabase("RMZDb_WaterMeterDetail")
                .Options;

            dbContext = new RMZAPIDbContext(_dbContextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(dbContext);

            _repository = new WaterMeterDetailRepository(dbContext);

        }
        [Fact]
        public async void AddWaterMeterDetail_ValidData_ReturnOkResult()
        {
            //arrange
            var waterMeterDetailController = new WaterMeterDetailController(_repository);
            var WaterMeter = dbContext.WaterMeters.Where(x => x.Number == "WaterMeter001").FirstOrDefault();
            var waterMeterDetail = new WaterMeterDetailRequest() { ReadingDate = DateTime.Now.AddDays(-30), WaterMeterId = WaterMeter.Id, StartReading = 0, EndReading = 100 };

            //act
            var data = await waterMeterDetailController.AddWaterMeterDetail(waterMeterDetail);

            //assert
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddWaterMeterDetail_InValidData_ReturnBadRequest()
        {
            //arrange
            var waterMeterDetailController = new WaterMeterDetailController(_repository);
            WaterMeterDetailRequest waterMeterDetail = null;

            //act
            var data = await waterMeterDetailController.AddWaterMeterDetail(waterMeterDetail);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddWaterMeterDetail_InValidData_Return_BadRequest()
        {
            //arrange
            var waterMeterDetailController = new WaterMeterDetailController(_repository);
            var waterMeterDetail = new WaterMeterDetailRequest() { ReadingDate = default, WaterMeterId = 0, StartReading = 0, EndReading = 0 };

            //act
            var data = await waterMeterDetailController.AddWaterMeterDetail(waterMeterDetail);

            //assert
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void AddWaterMeterDetail_ValidData_MatchResult()
        {
            //Arrange  
            var waterMeterDetailController = new WaterMeterDetailController(_repository);
            var WaterMeter = dbContext.WaterMeters.Where(x => x.Id == 4).FirstOrDefault();
            var waterMeterDetail = new WaterMeterDetailRequest() { ReadingDate = DateTime.Now, WaterMeterId = WaterMeter.Id, StartReading = 100, EndReading = 200 };


            //Act  
            var data = await waterMeterDetailController.AddWaterMeterDetail(waterMeterDetail);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;

            var data2 = await waterMeterDetailController.GetWaterMeterDetail((int?)okResult.Value);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult2 = data2.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeterDetail = okResult2.Value.Should().BeAssignableTo<WaterMeterDetail>().Subject;

            Assert.Equal(DateTime.Now.ToShortDateString(), WaterMeterDetail.ReadingDate.ToShortDateString());
            Assert.Equal(WaterMeter.Id, WaterMeterDetail.WaterMeterId);
            Assert.Equal(100, WaterMeterDetail.StartReading);
            Assert.Equal(200, WaterMeterDetail.EndReading);
            Assert.Equal(100, WaterMeterDetail.TotalUnits);
        }

        [Fact]
        public async void AddWaterMeterDetail_ExistingData_ReturnOkResult()
        {
            //arrange
            var waterMeterDetailController = new WaterMeterDetailController(_repository);
            var WaterMeter = dbContext.WaterMeters.Where(x => x.Number == "WaterMeter001").FirstOrDefault();
            var waterMeterDetail = new WaterMeterDetailRequest() { ReadingDate = DateTime.Now.AddDays(-30), WaterMeterId = WaterMeter.Id, StartReading = 0, EndReading = 100 };
            var countBeforeAddition = await dbContext.WaterMeterDetails.CountAsync();

            //act
            var data = await waterMeterDetailController.AddWaterMeterDetail(waterMeterDetail);
            var countAfterAddition = await dbContext.WaterMeterDetails.CountAsync();

            //assert
            Assert.IsType<OkObjectResult>(data);
            Assert.Equal(countBeforeAddition, countAfterAddition);
        }

        #region Get By Id  

        [Fact]
        public async void GetWaterMeterDetailById_ValidData_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeterDetailId = 1;

            //Act  
            var data = await controller.GetWaterMeterDetail(WaterMeterDetailId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetWaterMeterDetailById_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeterDetailId = 10;

            //Act  
            var data = await controller.GetWaterMeterDetail(WaterMeterDetailId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetWaterMeterDetailById_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            int? WaterMeterDetailId = null;

            //Act  
            var data = await controller.GetWaterMeterDetail(WaterMeterDetailId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetWaterMeterDetailById_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            int? WaterMeterDetailId = 1;

            //Act  
            var data = await controller.GetWaterMeterDetail(WaterMeterDetailId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeterDetail = okResult.Value.Should().BeAssignableTo<WaterMeterDetail>().Subject;

            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), WaterMeterDetail.ReadingDate.ToShortDateString());
            Assert.Equal(1, WaterMeterDetail.WaterMeterId);
            Assert.Equal(0, WaterMeterDetail.StartReading);
            Assert.Equal(100, WaterMeterDetail.EndReading);
            Assert.Equal(100, WaterMeterDetail.TotalUnits);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetWaterMeterDetails_Return_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);

            //Act  
            var data = await controller.GetWaterMeterDetails();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void GetWaterMeterDetails_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);

            //Act  
            var data = await controller.GetWaterMeterDetails();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeterDetail = okResult.Value.Should().BeAssignableTo<List<WaterMeterDetail>>().Subject;

            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), WaterMeterDetail[0].ReadingDate.ToShortDateString());
            Assert.Equal(4, WaterMeterDetail[0].WaterMeterId);
            Assert.Equal(10, WaterMeterDetail[0].StartReading);
            Assert.Equal(100, WaterMeterDetail[0].EndReading);
            Assert.Equal(90, WaterMeterDetail[0].TotalUnits);
        }

        #endregion

        #region Get By WaterMeterId  

        [Fact]
        public async void GetWaterMeterDetailByWaterMeterId_ValidData_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeter = dbContext.WaterMeters.Where(x => x.Number == "WaterMeter002").FirstOrDefault();

            //Act  
            var data = await controller.GetWaterMeterDetailsByWaterMeter(WaterMeter.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetWaterMeterDetailByWaterMeterId_InvalidData_NotFoundResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeterId = 10;

            //Act  
            var data = await controller.GetWaterMeterDetailsByWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetWaterMeterDetailByWaterMeterId_Null_BadRequestResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            int? WaterMeterId = null;

            //Act  
            var data = await controller.GetWaterMeterDetailsByWaterMeter(WaterMeterId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void GetWaterMeterDetailByWaterMeterId_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeter = dbContext.WaterMeters.Where(x => x.Number == "WaterMeter002").FirstOrDefault();

            //Act  
            var data = await controller.GetWaterMeterDetailsByWaterMeter(WaterMeter.Id);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeterDetail = okResult.Value.Should().BeAssignableTo<List<WaterMeterDetail>>().Subject;

            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), WaterMeterDetail[0].ReadingDate.ToShortDateString());
            Assert.Equal(WaterMeter.Id, WaterMeterDetail[0].WaterMeterId);
            Assert.Equal(10, WaterMeterDetail[0].StartReading);
            Assert.Equal(100, WaterMeterDetail[0].EndReading);
            Assert.Equal(90, WaterMeterDetail[0].TotalUnits);
        }

        #endregion

        #region WaterMeterDetail By different Param
        [Fact]
        public async void GetWaterMeterDetailByParams_FacilityId_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var Facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();

            //Act  
            var data = await controller.GetWaterMeterDetailByParams(Facility.Id, null, null, null, null, null, null);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeterDetail = okResult.Value.Should().BeAssignableTo<List<DisplayWaterMeterDetail>>().Subject;

            Assert.Equal("Carmelram", WaterMeterDetail[0].facilityname);
            Assert.Equal("Building001", WaterMeterDetail[0].buildingname);
            Assert.Equal("Zone002", WaterMeterDetail[0].zonename);
            Assert.Equal(10, WaterMeterDetail[0].startunit);
            Assert.Equal(100, WaterMeterDetail[0].endunit);
            Assert.Equal(90, WaterMeterDetail[0].totalunits);
            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), WaterMeterDetail[0].readingdate.ToShortDateString());
            Assert.Equal("WaterMeter002", WaterMeterDetail[0].watermeter);
        }

        [Fact]
        public async void GetWaterMeterDetailByParams_BuildingId_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();
            var building = dbContext.Buildings.Where(x => x.Name == "Building001" && x.FacilityId == facility.Id).FirstOrDefault();

            //Act  
            var data = await controller.GetWaterMeterDetailByParams(null, building.Id, null, null, null, null, null);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeterDetail = okResult.Value.Should().BeAssignableTo<List<DisplayWaterMeterDetail>>().Subject;

            Assert.Equal("Carmelram", WaterMeterDetail[0].facilityname);
            Assert.Equal("Building001", WaterMeterDetail[0].buildingname);
            Assert.Equal("Zone002", WaterMeterDetail[0].zonename);
            Assert.Equal(10, WaterMeterDetail[0].startunit);
            Assert.Equal(100, WaterMeterDetail[0].endunit);
            Assert.Equal(90, WaterMeterDetail[0].totalunits);
            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), WaterMeterDetail[0].readingdate.ToShortDateString());
            Assert.Equal("WaterMeter002", WaterMeterDetail[0].watermeter);
        }

        [Fact]
        public async void GetWaterMeterDetailByParams_ZoneId_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var facility = dbContext.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault();
            var building = dbContext.Buildings.Where(x => x.Name == "Building001" && x.FacilityId == facility.Id).FirstOrDefault();
            var floor = dbContext.Floors.Where(x => x.Name == "Floor001" && x.BuildingId == building.Id).FirstOrDefault();
            var zone = dbContext.Zones.Where(x => x.Name == "Zone002" && x.FloorId == floor.Id).FirstOrDefault();
            //Act  
            var data = await controller.GetWaterMeterDetailByParams(null, null, null, zone.Id, null, null, null);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeterDetail = okResult.Value.Should().BeAssignableTo<List<DisplayWaterMeterDetail>>().Subject;

            Assert.Equal("Carmelram", WaterMeterDetail[0].facilityname);
            Assert.Equal("Building001", WaterMeterDetail[0].buildingname);
            Assert.Equal("Zone002", WaterMeterDetail[0].zonename);
            Assert.Equal(10, WaterMeterDetail[0].startunit);
            Assert.Equal(100, WaterMeterDetail[0].endunit);
            Assert.Equal(90, WaterMeterDetail[0].totalunits);
            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), WaterMeterDetail[0].readingdate.ToShortDateString());
            Assert.Equal("WaterMeter002", WaterMeterDetail[0].watermeter);
        }

        [Fact]
        public async void GetWaterMeterDetailByParams_DateRange_MatchResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var startDate = DateTime.Now.AddDays(-30);
            var endDate = DateTime.Now;
            //Act  
            var data = await controller.GetWaterMeterDetailByParams(null, null, null, null, null, startDate, endDate);

            //Assert             
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var WaterMeterDetail = okResult.Value.Should().BeAssignableTo<List<DisplayWaterMeterDetail>>().Subject;

            Assert.Equal(4, WaterMeterDetail.Count());
            Assert.Equal("Carmelram", WaterMeterDetail[0].facilityname);
            Assert.Equal("Building001", WaterMeterDetail[0].buildingname);
            Assert.Equal("Zone002", WaterMeterDetail[0].zonename);
            Assert.Equal(10, WaterMeterDetail[0].startunit);
            Assert.Equal(100, WaterMeterDetail[0].endunit);
            Assert.Equal(90, WaterMeterDetail[0].totalunits);
            Assert.Equal(DateTime.Now.AddDays(-30).ToShortDateString(), WaterMeterDetail[0].readingdate.ToShortDateString());
            Assert.Equal("WaterMeter002", WaterMeterDetail[0].watermeter);
        }

        [Fact]
        public async void GetWaterMeterDetailByParams_OkObject()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);

            //Act  
            var data = await controller.GetWaterMeterDetailByParams(null, null, null, null, null, null, null);

            //Assert             
            Assert.IsType<OkObjectResult>(data);
        }
        #endregion

        #region Update

        [Fact]
        public async void Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);

            var WaterMeterDetailId = 3;
            var WaterMeterDetail = new WaterMeterDetailRequest();

            WaterMeterDetail.ReadingDate = DateTime.Now;
            WaterMeterDetail.StartReading = 10;
            WaterMeterDetail.EndReading = 250;
            WaterMeterDetail.WaterMeterId = dbContext.WaterMeters.Where(x => x.Number == "WaterMeter002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateWaterMeterDetail(WaterMeterDetailId, WaterMeterDetail);

            //Assert  
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeterDetailId = 1;
            var WaterMeterDetail = new WaterMeterDetailRequest();

            WaterMeterDetail.ReadingDate = default;
            WaterMeterDetail.StartReading = 0;
            WaterMeterDetail.EndReading = 0;
            WaterMeterDetail.WaterMeterId = dbContext.WaterMeters.Where(x => x.Number == "WaterMeter002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateWaterMeterDetail(WaterMeterDetailId, WaterMeterDetail);


            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidData_Return_NotFound()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeterDetailId = 10;
            var WaterMeterDetail = new WaterMeterDetailRequest();

            WaterMeterDetail.ReadingDate = DateTime.Now;
            WaterMeterDetail.StartReading = 10;
            WaterMeterDetail.EndReading = 250;
            WaterMeterDetail.WaterMeterId = dbContext.WaterMeters.Where(x => x.Number == "WaterMeter002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateWaterMeterDetail(WaterMeterDetailId, WaterMeterDetail);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(updatedData);
        }

        [Fact]
        public async void Update_InvalidDataAsZero_Return_BadRequest()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeterDetailId = 0;
            var WaterMeterDetail = new WaterMeterDetailRequest();

            WaterMeterDetail.ReadingDate = DateTime.Now;
            WaterMeterDetail.StartReading = 10;
            WaterMeterDetail.EndReading = 250;
            WaterMeterDetail.WaterMeterId = dbContext.WaterMeters.Where(x => x.Number == "WaterMeter002").FirstOrDefault().Id;
            //Act              
            var updatedData = await controller.UpdateWaterMeterDetail(WaterMeterDetailId, WaterMeterDetail);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        #endregion



        #region Delete WaterMeterDetail  

        [Fact]
        public async void Delete_WaterMeterDetail_Return_OkResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeterDetailId = 2;

            //Act  
            var data = await controller.DeleteWaterMeterDetail(WaterMeterDetailId);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_WaterMeterDetail_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            var WaterMeterDetailId = 5;

            //Act  
            var data = await controller.DeleteWaterMeterDetail(WaterMeterDetailId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Delete_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new WaterMeterDetailController(_repository);
            int? WaterMeterDetailId = null;

            //Act  
            var data = await controller.DeleteWaterMeterDetail(WaterMeterDetailId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}

