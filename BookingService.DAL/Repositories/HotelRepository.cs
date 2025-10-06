using BookingService.Common.Entities;
using BookingService.DAL.Data;
using BookingService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL.Repositories
{
    public class HotelRepository : GeneratorRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(HotelServiceDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Hotel>> GetHotelsByCityAsync(string city)
        {
            return await _dbSet
                .Include(h => h.Rooms)
                .Where(h => h.City.ToLower().Contains(city.ToLower()))
                .ToListAsync();
        }

        public async Task<Hotel?> GetHotelWithRoomsAsync(int hotelId)
        {
            return await _dbSet
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == hotelId);
        }
    }
}
