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
            return await httpClient.GetFromJsonAsync<List<Reservation>>("/api/reservation/allreservation");
        }
        public async Task<List<ReservationRoom>> GetAllReservationRoomAsync()
        {
            var res = await httpClient.GetFromJsonAsync<List<ReservationRoom>>("/api/room/allreservationroom");
            return res;
        }

        /*public async Task<List<RoomType>> GetAllRoomTypeAsync()
        {
            return await httpClient.GetFromJsonAsync<List<RoomType>>("api/reservation/allroomtype");
        }*/
        public async Task<List<Guest>> GetAllGuestAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Guest>>("/api/reservation/allreservationroom");
        }

        public async Task<List<RoomType>> GetAvailableRoomTypesAsync(DateTime start, DateTime end)
        {
            return await httpClient.GetFromJsonAsync<List<RoomType>>($"/api/room/availableRoomTypes/start/end");
        }

        public async Task<List<Room>> GetAvailableRooms(DateTime start, DateTime end)
        {
            return await httpClient.GetFromJsonAsync<List<Room>>($"/api/room/availableRoom/start/end");
        }

        public async Task<List<RoomCleaningInfo>> GetCleanRoomsAsync()
        {
            return await httpClient.GetFromJsonAsync<List<RoomCleaningInfo>>($"/api/room/cleanrooms");
        }
    }
}
