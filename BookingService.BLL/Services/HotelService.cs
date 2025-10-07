using BookingService.BLL.Services.Interfaces;
using BookingService.Common.DTOs;
using BookingService.Common.DTOs.CrudDTOs;
using BookingService.Common.Entities;
using BookingService.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.BLL.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<HotelDto>> GetAllHotelsAsync()
        {
            var hotels = await _hotelRepository.GetAllAsync();
            return hotels.Select(MapToDto);
        }

        public async Task<HotelDto?> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetHotelWithRoomsAsync(id);
            return hotel != null ? MapToDtoWithRooms(hotel) : null;
        }

        public async Task<IEnumerable<HotelDto>> GetHotelsByCityAsync(string city)
        {
            var hotels = await _hotelRepository.GetHotelsByCityAsync(city);
            return hotels.Select(MapToDtoWithRooms);
        }

        public async Task<HotelDto> CreateHotelAsync(CreateHotelDto createHotelDto)
        {
            var hotel = new Hotel
            {
                Name = createHotelDto.Name,
                Address = createHotelDto.Address,
                City = createHotelDto.City,
                Description = createHotelDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var createdHotel = await _hotelRepository.AddAsync(hotel);
            return MapToDto(createdHotel);
        }

        public async Task<HotelDto?> UpdateHotelAsync(int id, UpdateHotelDto updateHotelDto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null) return null;

            hotel.Name = updateHotelDto.Name;
            hotel.Address = updateHotelDto.Address;
            hotel.City = updateHotelDto.City;
            hotel.Description = updateHotelDto.Description;

            await _hotelRepository.UpdateAsync(hotel);
            return MapToDto(hotel);
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null) return false;

            await _hotelRepository.DeleteAsync(hotel);
            return true;
        }

        private static HotelDto MapToDto(Hotel hotel)
        {
            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                Description = hotel.Description
            };
        }

        private static HotelDto MapToDtoWithRooms(Hotel hotel)
        {
            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                City = hotel.City,
                Description = hotel.Description,
                Rooms = hotel.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    HotelId = r.HotelId,
                    Number = r.Number,
                    Type = r.Type,
                    PricePerNight = r.PricePerNight,
                    Capacity = r.Capacity,
                    Description = r.Description,
                    IsAvailable = r.IsAvailable,
                    HotelName = hotel.Name
                }).ToList()
            };
        }
        public async Task<IEnumerable<HotelDto>> GetAvailableHotelsByCityAsync(string city, DateTime? checkIn = null, DateTime? checkOut = null)
        {
            var hotels = await _hotelRepository.GetHotelsByCityAsync(city);

            if (!checkIn.HasValue || !checkOut.HasValue)
            {
                return hotels.Select(MapToDtoWithRooms);
            }

            var availableHotels = new List<Hotel>();

            foreach (var hotel in hotels)
            {
                var availableRooms = hotel.Rooms.Where(room =>
                    room.IsAvailable &&
                    !room.Bookings.Any(booking =>
                        booking.Status != BookingStatus.Cancelled &&
                        booking.CheckInDate < checkOut &&
                        booking.CheckOutDate > checkIn)).ToList();

                if (availableRooms.Any())
                {
                    var hotelWithAvailableRooms = new Hotel
                    {
                        Id = hotel.Id,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        City = hotel.City,
                        Description = hotel.Description,
                        CreatedAt = hotel.CreatedAt,
                        Rooms = availableRooms
                    };
                    availableHotels.Add(hotelWithAvailableRooms);
                }
            }

            return availableHotels.Select(MapToDtoWithRooms);
        }
    }
}
