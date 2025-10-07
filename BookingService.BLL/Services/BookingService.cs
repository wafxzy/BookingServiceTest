using BookingService.BLL.Services.Interfaces;
using BookingService.Common.DTOs;
using BookingService.Common.DTOs.CrudDTOs;
using BookingService.Common.Entities;
using BookingService.DAL.Repositories.Interfaces;

namespace BookingService.BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Select(MapToDto);
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return booking != null ? MapToDto(booking) : null;
        }

        public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId)
        {
            var bookings = await _bookingRepository.GetUserBookingsAsync(userId);
            return bookings.Select(MapToDto);
        }

        public async Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto createBookingDto)
        {
            Console.WriteLine($"Creating booking for userId: '{userId}'");
            
            var room = await _roomRepository.GetRoomWithBookingsAsync(createBookingDto.RoomId);
            if (room == null)
                throw new ArgumentException("Room not found", nameof(createBookingDto.RoomId));

            if (!room.IsAvailable)
                throw new InvalidOperationException("Room is not available");

            var isAvailable = await _bookingRepository.IsRoomAvailableAsync(
                createBookingDto.RoomId,
                createBookingDto.CheckInDate,
                createBookingDto.CheckOutDate);

            if (!isAvailable)
                throw new InvalidOperationException("Room is not available for the selected dates");

            var nights = (createBookingDto.CheckOutDate - createBookingDto.CheckInDate).Days;
            var totalPrice = nights * room.PricePerNight;

            var booking = new Booking
            {
                UserId = userId,
                RoomId = createBookingDto.RoomId,
                CheckInDate = createBookingDto.CheckInDate,
                CheckOutDate = createBookingDto.CheckOutDate,
                TotalPrice = totalPrice,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            Console.WriteLine($"About to create booking with UserId: '{booking.UserId}', RoomId: {booking.RoomId}");

            var createdBooking = await _bookingRepository.AddAsync(booking);

            createdBooking.Room = room;

            return MapToDto(createdBooking);
        }

        public async Task<BookingDto?> UpdateBookingStatusAsync(int id, UpdateBookingStatusDto updateStatusDto)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null) return null;

            booking.Status = updateStatusDto.Status;
            await _bookingRepository.UpdateAsync(booking);

            return MapToDto(booking);
        }

        public async Task<bool> CancelBookingAsync(int id, string userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null || booking.UserId != userId) return false;

            if (booking.Status == BookingStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed booking");

            booking.Status = BookingStatus.Cancelled;
            await _bookingRepository.UpdateAsync(booking);

            return true;
        }

        private static BookingDto MapToDto(Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                RoomId = booking.RoomId,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt,
                UserEmail = booking.User?.Email ?? string.Empty,
                RoomNumber = booking.Room?.Number ?? string.Empty,
                HotelName = booking.Room?.Hotel?.Name ?? string.Empty
            };
        }
    }
}
