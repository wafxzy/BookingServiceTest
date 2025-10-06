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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;

        public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return rooms.Select(MapToDto);
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetRoomWithBookingsAsync(id);
            return room != null ? MapToDto(room) : null;
        }

        public async Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(int hotelId, DateTime checkIn, DateTime checkOut)
        {
            var rooms = await _roomRepository.GetAvailableRoomsAsync(hotelId, checkIn, checkOut);
            return rooms.Select(MapToDto);
        }

        public async Task<IEnumerable<RoomDto>> SearchRoomsAsync(string city, DateTime checkIn, DateTime checkOut)
        {
            var rooms = await _roomRepository.SearchRoomsAsync(city, checkIn, checkOut);
            return rooms.Select(MapToDto);
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto)
        {
            var hotel = await _hotelRepository.GetByIdAsync(createRoomDto.HotelId);
            if (hotel == null)
                throw new ArgumentException("Hotel not found", nameof(createRoomDto.HotelId));

            var room = new Room
            {
                HotelId = createRoomDto.HotelId,
                Number = createRoomDto.Number,
                Type = createRoomDto.Type,
                PricePerNight = createRoomDto.PricePerNight,
                Capacity = createRoomDto.Capacity,
                Description = createRoomDto.Description,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdRoom = await _roomRepository.AddAsync(room);

            // Load hotel information for DTO
            createdRoom.Hotel = hotel;

            return MapToDto(createdRoom);
        }

        public async Task<RoomDto?> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto)
        {
            var room = await _roomRepository.GetRoomWithBookingsAsync(id);
            if (room == null) return null;

            room.Number = updateRoomDto.Number;
            room.Type = updateRoomDto.Type;
            room.PricePerNight = updateRoomDto.PricePerNight;
            room.Capacity = updateRoomDto.Capacity;
            room.Description = updateRoomDto.Description;
            room.IsAvailable = updateRoomDto.IsAvailable;

            await _roomRepository.UpdateAsync(room);
            return MapToDto(room);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return false;

            await _roomRepository.DeleteAsync(room);
            return true;
        }

        private static RoomDto MapToDto(Room room)
        {
            return new RoomDto
            {
                Id = room.Id,
                HotelId = room.HotelId,
                Number = room.Number,
                Type = room.Type,
                PricePerNight = room.PricePerNight,
                Capacity = room.Capacity,
                Description = room.Description,
                IsAvailable = room.IsAvailable,
                HotelName = room.Hotel?.Name ?? string.Empty
            };
        }
    }
}
