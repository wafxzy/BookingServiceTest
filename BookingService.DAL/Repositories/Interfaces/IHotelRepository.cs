using BookingService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.DAL.Repositories.Interfaces
{
    public interface IHotelRepository : IGeneratorRepository<Hotel>
    {
        Task<IEnumerable<Hotel>> GetHotelsByCityAsync(string city);
        Task<Hotel?> GetHotelWithRoomsAsync(int hotelId);
    }
}
