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
    public class BookingRepository : GeneratorRepository<Booking>, IBookingRepository
    {
        public BookingRepository(HotelServiceDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _dbSet
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _dbSet
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                .Include(b => b.User)
                .Where(b => b.CheckInDate >= startDate && b.CheckOutDate <= endDate)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var conflictingBooking = await _dbSet
                .Where(b => b.RoomId == roomId && b.Status != BookingStatus.Cancelled)
                .Where(b => (b.CheckInDate <= checkIn && b.CheckOutDate > checkIn) ||
                           (b.CheckInDate < checkOut && b.CheckOutDate >= checkOut) ||
                           (b.CheckInDate >= checkIn && b.CheckOutDate <= checkOut))
                .FirstOrDefaultAsync();

            return conflictingBooking == null;
        }
    }
}
