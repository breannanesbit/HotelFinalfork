using FluentAssertions;
using HotelFinal.Server.Controllers;
using HotelFinal.Server;
using HotelFinal.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace HotelFinal.Test
{
    public class Tests
    {
        public RoomController RoomController { get; set; }
        [SetUp]
        public void Setup()
        {
            HotelContext hc = new();
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<RoomController>();

            RoomController = new(hc, logger);
        }

        [Test]
        public void TestGetAvailableRoomTypes()
        {
            var roomTypeCounts = new Dictionary<int, int>()
            {
                {1, 1},
                {2, 1},
                {3, 1},
            };

            var reservations = new List<Reservation>()
            {
                new Reservation()
                {
                    Id = 1,
                    GuestId = 1,
                    ExpectedCheckin = new DateOnly(2022, 12, 1),
                    ExpectedCheckout = new DateOnly(2022,12, 2),
                },
                new Reservation()
                {
                    Id = 2,
                    GuestId = 2,
                    ExpectedCheckin = new DateOnly(2022, 12, 1),
                    ExpectedCheckout = new DateOnly(2022,12, 2),
                },
                new Reservation()
                {
                    Id = 3,
                    GuestId = 3,
                    ExpectedCheckin = new DateOnly(2022, 12, 1),
                    ExpectedCheckout = new DateOnly(2022,12, 2),
                },
            };

            var reservationRooms = new List<ReservationRoom>()
            {
                new ReservationRoom()
                {
                    Id = 1,
                    ReservationId = 1,
                    RoomTypeId = 1
                },
                new ReservationRoom()
                {
                    Id = 2,
                    ReservationId = 2,
                    RoomTypeId = 2
                },
                new ReservationRoom()
                {
                    Id = 3,
                    ReservationId = 3,
                    RoomTypeId = 3
                },
            };

            var availableRooms = RoomController.GetAvalibleRoomCounts(roomTypeCounts, reservations, reservationRooms);
            availableRooms.Should().Equal(
                new Dictionary<int, int>()
                {
                    {1, 0},
                    {2, 0},
                    {3, 0}
                });
        }

        [Test]
        public void TestFilterRoomTypes()
        {
            var roomTypeCounts = new Dictionary<int, int>()
            {
                {1, 1},
                {2, 1},
                {3, 0},
            };

            var roomTypes = new List<RoomType>()
            {
                new RoomType()
                {
                    Id = 1,
                    RType = "Room Type 1",
                    BaseRentalRate = 0.0m,
                    Smoking = false
                },
                new RoomType()
                {
                    Id = 2,
                    RType = "Room Type 2",
                    BaseRentalRate = 0.0m,
                    Smoking = false
                },
                new RoomType()
                {
                    Id = 3,
                    RType = "Room Type 3",
                    BaseRentalRate = 0.0m,
                    Smoking = false
                },
            };

            var filteredRoomTypes = RoomController.FilterEmptyRoomTypes(roomTypeCounts, roomTypes);

            filteredRoomTypes.Should().BeEquivalentTo(
                new List<RoomType>()
                {
                    new RoomType()
                    {
                        Id = 1,
                        RType = "Room Type 1",
                        BaseRentalRate = 0.0m,
                        Smoking = false
                    },
                    new RoomType()
                    {
                        Id = 2,
                        RType = "Room Type 2",
                        BaseRentalRate = 0.0m,
                        Smoking = false
                    }
                }
                );
        }
    }
}