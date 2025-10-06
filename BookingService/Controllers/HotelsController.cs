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
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetAllHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
                return NotFound();

            return Ok(hotel);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<HotelDto>>> SearchHotels([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City parameter is required");

            var hotels = await _hotelService.GetHotelsByCityAsync(city);
            return Ok(hotels);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<HotelDto>> CreateHotel([FromBody] CreateHotelDto createHotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = await _hotelService.CreateHotelAsync(createHotelDto);
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<HotelDto>> UpdateHotel(int id, [FromBody] UpdateHotelDto updateHotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = await _hotelService.UpdateHotelAsync(id, updateHotelDto);
            if (hotel == null)
                return NotFound();

            return Ok(hotel);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            var result = await _hotelService.DeleteHotelAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
