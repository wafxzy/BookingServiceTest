using BookingService.Common.DTOs.CrudDTOs;
using BookingService.Common.DTOs;

namespace BookingService.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto?> GetBookingByIdAsync(int id);
        Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId);
        Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto);
        Task<BookingDto?> UpdateBookingStatusAsync(int id, UpdateBookingStatusDto updateStatusDto);
        Task<bool> CancelBookingAsync(int id, string userId);
    }
}
