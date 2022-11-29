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
    }
}
