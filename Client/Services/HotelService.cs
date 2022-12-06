using HotelFinal.Client.Pages;
using HotelFinal.Shared;
using System.Collections;
using System.Net.Http.Json;

namespace HotelFinal.Client.Services
{
    public class HotelService
    {
        private readonly HttpClient httpClient;

        public HotelService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        
        public async Task<List<RoomType>> GetAllRoomTypesAsync()
        {
            var rooms = await httpClient.GetFromJsonAsync<List<RoomType>>("/api/room/roomtype");
            return rooms;
        }

        public async Task<List<Reservation>> GetAllReservationAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Reservation>>("api/reservation/allreservation");
        }
        public async Task<List<ReservationRoom>> GetAllReservationRoomAsync()
        {
            return await httpClient.GetFromJsonAsync<List<ReservationRoom>>("api/reservation/allreservationroom");
        }

        public async Task<List<RoomType>> GetAllRoomTypeAsync()
        {
            return await httpClient.GetFromJsonAsync<List<RoomType>>("api/reservation/allroomtype");
        }
        public async Task<List<Guest>> GetAllGuestAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Guest>>("api/reservation/guest");
        }

        public async Task<List<RoomType>> GetAllRoomTypesAvailable(DateTime start, DateTime end)
        {
            return await httpClient.GetFromJsonAsync<List<RoomType>>($"/api/room/availableRoomTypes/2022-12-1/2022-12-2");
        }
    }
}
