using BookingService.BLL.Services.Interfaces;
using BookingService.Common.DTOs;
using BookingService.Common.DTOs.CrudDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound();

            return Ok(room);
        }

        [HttpGet("hotel/{hotelId}/available")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAvailableRooms(
            int hotelId,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            if (checkIn >= checkOut)
                return BadRequest("Check-in date must be before check-out date");

            if (checkIn < DateTime.Today)
                return BadRequest("Check-in date cannot be in the past");

            var rooms = await _roomService.GetAvailableRoomsAsync(hotelId, checkIn, checkOut);
            return Ok(rooms);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> SearchRooms(
            [FromQuery] string city,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City parameter is required");

            if (checkIn >= checkOut)
                return BadRequest("Check-in date must be before check-out date");

            if (checkIn < DateTime.Today)
                return BadRequest("Check-in date cannot be in the past");

            var rooms = await _roomService.SearchRoomsAsync(city, checkIn, checkOut);
            return Ok(rooms);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] CreateRoomDto createRoomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var room = await _roomService.CreateRoomAsync(createRoomDto);
                return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomDto>> UpdateRoom(int id, [FromBody] UpdateRoomDto updateRoomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = await _roomService.UpdateRoomAsync(id, updateRoomDto);
            if (room == null)
                return NotFound();

            return Ok(room);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            var result = await _roomService.DeleteRoomAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
