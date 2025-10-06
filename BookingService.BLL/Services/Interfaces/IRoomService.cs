using BookingService.Common.DTOs;
using BookingService.Common.DTOs.CrudDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.BLL.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
        Task<RoomDto?> GetRoomByIdAsync(int id);
        Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<RoomDto>> SearchRoomsAsync(string city, DateTime checkIn, DateTime checkOut);
        Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto);
        Task<RoomDto?> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto);
        Task<bool> DeleteRoomAsync(int id);
    }
}
