using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using ApplicationCore.Specifications;
using Infrastructure;
using Infrastructure.Data.AdoRepositories;
using Moq;
using NUnit.Framework;


namespace UnitTests.Core.Services
{
    [TestFixture]
    public class ParkingServiceTests
    {
        private IParkingService service;
        private CancellationToken token;
        private Mock<IRepository<ParkIn>> parkInRepository;
        private Mock<IRepository<ParkOut>> parkOutRepository;
        private Mock<ICommonRepository> commonRepository;
        private Mock<ParkInByTagNumber> parkInByTagNameSpecification;

        [SetUp]
        public void SetUp()
        {

            token = new CancellationToken();
                       
            parkInRepository = new Mock<IRepository<ParkIn>>();

            parkOutRepository = new Mock<IRepository<ParkOut>>();
            
            commonRepository = new Mock<ICommonRepository>();
            
            parkInByTagNameSpecification = new Mock<ParkInByTagNumber>("02201018");

        }

        [Test]
        public async Task CheckIn_Should_Success()
        {
            //arrange
            ParkIn parkIn = new ParkIn("02201018", DateTime.Now);         
            parkInRepository.Setup(pir => pir.AddAsync(parkIn, token)).Returns(Task.FromResult(parkIn));
            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object, parkInByTagNameSpecification.Object);

            // Act
            bool result = await service.CheckIn(parkIn.TagNumber);

            // Assert
            NUnit.Framework.Assert.IsTrue(result);

        }

        [Test]
        public async Task CheckOut_Should_Fail()
        {
            //arrange
            parkInRepository.Setup(pir => pir.GetAllAsync(parkInByTagNameSpecification.Object, token)).Returns(Task.FromResult(new List<ParkIn>()));
            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object, parkInByTagNameSpecification.Object);

            // Act
            bool result = await service.CheckOut("02201019", 15, 50);

            // Assert
            NUnit.Framework.Assert.IsFalse(result);

        }

        [Test]
        public async Task CheckOut_Should_Success()
        {
            //arrange
            Mock<IDbManager> dbManager = new Mock<IDbManager>();
            List<ParkIn> parkIns = new List<ParkIn>();
            ParkIn parkIn = new ParkIn("02201018", DateTime.Now);
            parkIns.Add(parkIn);

            parkInRepository.Setup(pir => pir.GetAllAsync(parkInByTagNameSpecification.Object, token)).Returns(Task.FromResult(parkIns));
            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object, parkInByTagNameSpecification.Object, dbManager.Object);

            // Act
            bool result = await service.CheckOut(parkIn.TagNumber, 15, 50);

            // Assert
            NUnit.Framework.Assert.True(result);

        }

        [Test]
        public async Task ParkInByTagNumber_Should_Fail()
        {
            //arrange
            parkInRepository.Setup(pir => pir.GetAllAsync(parkInByTagNameSpecification.Object, token)).Returns(Task.FromResult(new List<ParkIn>()));
            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object, parkInByTagNameSpecification.Object);

            // Act
            ParkIn result = await service.ParkInByTagNumber("02201019");

            // Assert
            NUnit.Framework.Assert.IsNull(result);
        }

        [Test]
        public async Task ParkInByTagNumber_Should_Success()
        {
            //arrange
            List<ParkIn> parkIns = new List<ParkIn>();
            ParkIn parkIn = new ParkIn("02201018", DateTime.Now);
            parkIns.Add(parkIn);
            parkInRepository.Setup(pir => pir.GetAllAsync(parkInByTagNameSpecification.Object, token)).Returns(Task.FromResult(parkIns));
            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object, parkInByTagNameSpecification.Object);

            // Act
            ParkIn result = await service.ParkInByTagNumber(parkIn.TagNumber);

            // Assert
            NUnit.Framework.Assert.AreEqual(parkIn.TagNumber, result.TagNumber);
        }

        [Test]
        public async Task IsCarAlreadyParked_Should_Fail()
        {
            //arrange
            parkInRepository.Setup(pir => pir.AnyAsync(parkInByTagNameSpecification.Object, token)).Returns(Task.FromResult(false));
            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object, parkInByTagNameSpecification.Object);

            // Act
            bool result = await service.IsCarAlreadyParked("02201019");

            // Assert
            NUnit.Framework.Assert.IsFalse(result);
        }

        [Test]
        public async Task IsCarAlreadyParked_Should_Success()
        {
            //arrange
            parkInRepository.Setup(pir => pir.AnyAsync(parkInByTagNameSpecification.Object, token)).Returns(Task.FromResult(true));
            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object, parkInByTagNameSpecification.Object);

            // Act
            bool result = await service.IsCarAlreadyParked("02201018");

            // Assert
            NUnit.Framework.Assert.IsTrue(result);
        }

        [Test]
        [TestCase(5)]
        [TestCase(8)]
        [TestCase(10)]
        public async Task AnySpotAvailable_Should_Fail(int maxSpot)
        {
            //arrange
            parkInRepository.Setup(pir => pir.CountAsync(null, token)).Returns(Task.FromResult(12));

            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object);

            // Act
            bool result = await service.AnySpotAvailable(maxSpot);

            // Assert
            NUnit.Framework.Assert.False(result);
        }

        [Test]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        public async Task AnySpotAvailable_Should_Success(int maxSpot)
        {
            //arrange
            parkInRepository.Setup(pir => pir.CountAsync(null, token)).Returns(Task.FromResult(5));

            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object);

            // Act
            bool result = await service.AnySpotAvailable(maxSpot);

            // Assert
            NUnit.Framework.Assert.True(result);
        }

        [Test]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        public async Task SpotAvailable_Should_Success(int maxSpot)
        {
            //arrange
            parkInRepository.Setup(pir => pir.CountAsync(null, token)).Returns(Task.FromResult(5));

            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object);

            // Act
            int result = await service.AvailableSpot(maxSpot);

            // Assert
            NUnit.Framework.Assert.AreEqual(result, maxSpot - 5);
        }

        [Test]
        public async Task AvailableSpot_Should_Success()
        {
            //arrange
            List<ParkIn> parkIns = new List<ParkIn>();
            ParkIn parkIn = new ParkIn("02201018", DateTime.Now);
            parkIns.Add(parkIn);
            parkIn = new ParkIn("02201019", DateTime.Now);
            parkIns.Add(parkIn);
            parkInRepository.Setup(pir => pir.GetAllAsync(null, token)).Returns(Task.FromResult(parkIns));

            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object);

            // Act
            List<ParkIn> result = await service.ParksInAsync();

            // Assert
            NUnit.Framework.Assert.AreEqual(result.Count, parkIns.Count);
        }

        [Test]
        public async Task TotalRevenueOfToday_Should_Success()
        {
            //arrange

            DateTime dateTime = DateTime.Now;

            commonRepository.Setup(cr => cr.RevenueOfTheDay(dateTime, 15, token)).Returns(Task.FromResult(Convert.ToDecimal(500)));

            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object);

            // Act
            decimal result = await service.RevenueOfTheDay(dateTime,15);

            // Assert
            NUnit.Framework.Assert.AreEqual(result, Convert.ToDecimal(500));
        }

        [Test]
        public async Task AverageNumberOfCar_Should_Success()
        {
            //arrange

            DateTime toDate = DateTime.Now.AddDays(-1);
            DateTime fromDate = toDate.AddDays(-30);

            commonRepository.Setup(cr => cr.AverageNumberOfCar(fromDate, toDate, token)).Returns(Task.FromResult(20));

            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object);

            // Act
            int result = await service.AverageNumberOfCar(fromDate, toDate);

            // Assert
            NUnit.Framework.Assert.AreEqual(result, 20);
        }

        [Test]
        public async Task AverageRevenue_Should_Success()
        {
            //arrange

            DateTime toDate = DateTime.Now.AddDays(-1);
            DateTime fromDate = toDate.AddDays(-30);

            commonRepository.Setup(cr => cr.AverageRevenue(fromDate, toDate, 15, token)).Returns(Task.FromResult(Convert.ToDecimal(100)));

            service = new ParkingService(parkInRepository.Object, parkOutRepository.Object, commonRepository.Object);

            // Act
            decimal result = await service.AverageRevenue(fromDate, toDate, 15);

            // Assert
            NUnit.Framework.Assert.AreEqual(result, 100);
        }

        [TearDown]
        public void TearDown()
        {
            service = null;


        }
    }
}
