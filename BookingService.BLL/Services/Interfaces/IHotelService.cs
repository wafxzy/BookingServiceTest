using BookingService.Common.DTOs;
using BookingService.Common.DTOs.CrudDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.BLL.Services.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelDto>> GetAllHotelsAsync();
        Task<HotelDto?> GetHotelByIdAsync(int id);
        Task<IEnumerable<HotelDto>> GetHotelsByCityAsync(string city);
        Task<HotelDto> CreateHotelAsync(CreateHotelDto createHotelDto);
        Task<HotelDto?> UpdateHotelAsync(int id, UpdateHotelDto updateHotelDto);
        Task<bool> DeleteHotelAsync(int id);
    }
}
