using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Moq.AutoMock;
using System.Linq.Expressions;
using TTB.CAVU.ParkingSpaces.DataAccess.Model;
using TTB.CAVU.ParkingSpaces.DataAccess.Repository;
using TTB.CAVU.ParkingSpaces.Services.Parking;
using Xunit;

namespace TTB.CAVU.ParkingSpaces.Services.Test
{
    public class ParkingService_GetAvailableSpaces
    {
        private IAppRepository _appRepository;

        public ParkingService_GetAvailableSpaces()
        {
            var spaceIds = new List<ObjectId>();
            for (int i = 0; i < 10; i++)
            {
                spaceIds.Add(ObjectId.GenerateNewId());
            }

            var mockSpacesRepo = new Mock<IGenericRepositoryService<ParkingSpace>>();
            mockSpacesRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ParkingSpace, bool>>>())).ReturnsAsync(spaceIds.Select(x => new ParkingSpace
            {
                Id = x
            }));
            var bookings = spaceIds.Select(x => new SpaceBooking
            {
                Id = ObjectId.GenerateNewId(),
                SpaceId = x,
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2020, 1, 31)
            }).ToList();

            // Add The Extra test data from march onwards
            bookings.Add(new SpaceBooking
            {
                Id = ObjectId.GenerateNewId(),
                SpaceId = spaceIds[0],
                StartDate = new DateTime(2020, 3, 1),
                EndDate = new DateTime(2020, 3, 30)
            });

            bookings.AddRange(spaceIds.Take(9).Select(x => new SpaceBooking
            {
                Id = ObjectId.GenerateNewId(),
                SpaceId = x,
                StartDate = new DateTime(2020, 4, 1),
                EndDate = new DateTime(2020, 4, 30)
            }));

            var mockSpaceBookingRepo = new Mock<IGenericRepositoryService<SpaceBooking>>();
            mockSpaceBookingRepo.Setup(x => x.Query())
                .Returns(() => bookings);

            var mockRepository = new Mock<IAppRepository>();
            mockRepository.Setup(x => x.GetRepository<SpaceBooking>()).Returns(mockSpaceBookingRepo.Object);
            mockRepository.Setup(x => x.GetRepository<ParkingSpace>()).Returns(mockSpacesRepo.Object);
            _appRepository = mockRepository.Object;
        }

        [Fact]
        public async Task GetAvailableSpacesAsync_5Days_Return5Days()
        {
            var repo = new ParkingService(_appRepository);
            var response = await repo.GetAvailableSpacesAsync(new DateTime(2020, 1, 5), new DateTime(2020, 1, 10));
            Assert.Equal(5, response.Count());
        }

        [Fact]
        public async Task GetAvailableSpacesAsync_5DaysFullBooked_Return5DaysNoSpaces()
        {
            var repo = new ParkingService(_appRepository);
            var response = await repo.GetAvailableSpacesAsync(new DateTime(2020, 1, 5), new DateTime(2020, 1, 10));
            Assert.Equal(5, response.Count());

            foreach (var space in response)
            {
                Assert.Equal(0, space.AvailableSpaces);
            }
        }

        [Fact]
        public async Task GetAvailableSpacesAsync_2Of5DaysBooked_Returns5DaysWith2Have0And3Have10()
        {
            var repo = new ParkingService(_appRepository);
            var response = await repo.GetAvailableSpacesAsync(new DateTime(2020, 1, 30), new DateTime(2020, 2, 4));
            Assert.Equal(5, response.Count());
            Assert.Equal(new int[] { 0, 0, 10, 10, 10 }, response.Select(x => x.AvailableSpaces));
        }

        [Fact]
        public async Task GetAvailableSpacesAsync_5DaysFull_Returns5DaysWithNoSpacesAvailable()
        {
            var repo = new ParkingService(_appRepository);
            var response = await repo.GetAvailableSpacesAsync(new DateTime(2020, 1, 5), new DateTime(2020, 1, 10));
            Assert.Equal(5, response.Count());
            Assert.Equal(new int[] { 0,0,0,0,0 }, response.Select(x => x.AvailableSpaces));
        }
    }
}