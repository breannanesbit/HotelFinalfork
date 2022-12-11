using HotelFinal.Client.Pages;
using HotelFinal.Shared;
using System.Collections;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace HotelFinal.Client.Services
{
    public class HotelService
    {
        private readonly HttpClient httpClient;
        private readonly PublicClient publicClient;
        private readonly ILogger<HotelService> logger;

        public HotelService(HttpClient httpClient, PublicClient publicClient, ILogger<HotelService> logger)
        {
            this.httpClient = httpClient;
            this.publicClient = publicClient;
            this.logger = logger;
        }
        
        // Rooms
        // -----
        public async Task<List<RoomType>> GetAllRoomTypesAsync()
        {
            var rooms = await publicClient.Client.GetFromJsonAsync<List<RoomType>>("/api/room/roomtype");
            return rooms;
        }

        public async Task<List<RoomType>> GetAvailableRoomTypesAsync(DateTime start, DateTime end)
        {
            var s = start.ToString("yyyy-MM-dd");
            var e = end.ToString("yyyy-MM-dd");
            logger.LogInformation(s, e);
            return await httpClient.GetFromJsonAsync<List<RoomType>>($"/api/room/availableRoomTypes/{s}/{e}");
        }

        public async Task<List<RentalRoom>> GetAvailableRooms(DateTime start, DateTime end)
        {
            var s = start.ToString("yyyy-MM-dd");
            var e = end.ToString("yyyy-MM-dd");
            logger.LogInformation(s, e);
            return await httpClient.GetFromJsonAsync<List<RentalRoom>>($"/api/room/availableRoom/{s}/{e}");
        }

        public async Task<bool> IsValidRoom(int roomNumber)
        {
            return await httpClient.GetFromJsonAsync<bool>($"/api/room/valid/{roomNumber}");
            logger.LogWarning("could be invaild");
          
        }

        public async Task<Room> GetRoomFromRoomNumberAsync(int roomNumber)
        {
            return await httpClient.GetFromJsonAsync<Room>($"/api/room/{roomNumber}");
        }

        /*public async Task<List<RoomType>> GetAllRoomTypeAsync()
        {
            return await httpClient.GetFromJsonAsync<List<RoomType>>("api/reservation/allroomtype");
        }*/

        // Reservations
        // ------------
        public async Task<List<Reservation>> GetAllReservationAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Reservation>>("/api/Reservation/allreservation");
        }

        public async Task<List<ReservationRoom>> GetAllReservationRoomAsync()
        {
            var res = await httpClient.GetFromJsonAsync<List<ReservationRoom>>("/api/room/allreservationroom");
            return res;
        }

        public async Task PostReservationsAsync(ReservationPostObject rpo)
        {
            await httpClient.PostAsJsonAsync<ReservationPostObject>("/api/reservation", rpo);
        }

        public async Task SendReservationConfirmation(ReservationConfirmationObject rco)
        {
            await httpClient.PostAsJsonAsync<ReservationConfirmationObject>("/api/email/reservationConfirmation", rco);
        }

        public async Task<List<Reservation>> AllReservationsWithoutRentals()
        {
            return await httpClient.GetFromJsonAsync<List<Reservation>>("/api/Reservation/noRentalAllReservations");
        }


        // Guests
        // ------
        public async Task<Guest> GetGuestAsync(string firstname, string lastname)
        {
            Guest guest = await httpClient.GetFromJsonAsync<Guest>($"/api/guest/{firstname}/{lastname}");
            return guest;
        }

        public async Task<List<Guest>> GetAllGuestAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Guest>>("/api/reservation/allreservationroom");
        }

        public async Task PostGuestAsync(Guest guest)
        {
            await httpClient.PostAsJsonAsync<Guest>("/api/guest", guest);
        }

        // Staff
        // -----
        public async Task<Staff> GetStaffAsync(string firstname, string lastname)
        {
            Staff staff = await httpClient.GetFromJsonAsync<Staff>($"/api/staff/{firstname}/{lastname}");
            return staff;
        }

        public async Task PostStaffAsync(Staff staff)
        {
            await httpClient.PostAsJsonAsync<Staff>("/api/staff", staff);
        }


        // Cleaning
        // --------
        public async Task<List<RoomCleaningInfo>> GetCleanRoomsAsync()
        {
            return await httpClient.GetFromJsonAsync<List<RoomCleaningInfo>>($"/api/room/cleanrooms");
            logger.LogWarning("Could potiental be none");
        }

        public async Task<List<CleaningType>> GetCleaningTypesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<CleaningType>>("/api/cleaning/types");
        }

        public async Task RecordRoomCleaning(RoomCleaning roomCleaning)
        {
            await httpClient.PostAsJsonAsync<RoomCleaning>("/api/cleaning", roomCleaning);
        }

        // Rental
        // -------------
        public async Task CreateRentalAsync(Reservation reservation)
        {
            await httpClient.PostAsJsonAsync<Reservation>("/api/rental", reservation);
        }

        public async Task<Rental> GetReservationRental(int reservationId)
        {
            return await httpClient.GetFromJsonAsync<Rental>($"/api/rental/{reservationId}");
        }
    }
}
