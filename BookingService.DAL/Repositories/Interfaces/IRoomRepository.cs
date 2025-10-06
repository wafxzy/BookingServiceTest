using BookingService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.DAL.Repositories.Interfaces
{
    public interface IRoomRepository : IGeneratorRepository<Room>
    {
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<Room>> SearchRoomsAsync(string city, DateTime checkIn, DateTime checkOut);
        Task<Room?> GetRoomWithBookingsAsync(int roomId);
    }
}
