using BookingService.Common.Entities;
using BookingService.DAL.Data;
using BookingService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.DAL.Repositories
{
    public class RoomRepository : GeneratorRepository<Room>, IRoomRepository
    {
        public RoomRepository(HotelServiceDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut)
        {
            return await _dbSet
                .Include(r => r.Hotel)
                .Where(r => r.HotelId == hotelId && r.IsAvailable)
                .Where(r => !r.Bookings.Any(b =>
                    b.Status != BookingStatus.Cancelled &&
                    ((b.CheckInDate <= checkIn && b.CheckOutDate > checkIn) ||
                     (b.CheckInDate < checkOut && b.CheckOutDate >= checkOut) ||
                     (b.CheckInDate >= checkIn && b.CheckOutDate <= checkOut))))
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> SearchRoomsAsync(string city, DateTime checkIn, DateTime checkOut)
        {
            return await _dbSet
                .Include(r => r.Hotel)
                .Where(r => r.Hotel.City.ToLower().Contains(city.ToLower()) && r.IsAvailable)
                .Where(r => !r.Bookings.Any(b =>
                    b.Status != BookingStatus.Cancelled &&
                    ((b.CheckInDate <= checkIn && b.CheckOutDate > checkIn) ||
                     (b.CheckInDate < checkOut && b.CheckOutDate >= checkOut) ||
                     (b.CheckInDate >= checkIn && b.CheckOutDate <= checkOut))))
                .ToListAsync();
        }

        public async Task<Room?> GetRoomWithBookingsAsync(int roomId)
        {
            return await _dbSet
                .Include(r => r.Hotel)
                .Include(r => r.Bookings)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }
    }
}
