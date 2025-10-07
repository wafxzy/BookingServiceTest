using BookingService.BLL.Services.Interfaces;
using BookingService.Common.DTOs;
using BookingService.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<ActionResult<StatisticsDto>> GetStatistics()
        {
            var statistics = await _statisticsService.GetStatisticsAsync();
            return Ok(statistics);
        }

        [HttpGet("bookings-by-period")]
        public async Task<ActionResult<int>> GetBookingsByPeriod(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (startDate >= endDate)
                return BadRequest("Start date must be before end date");

            var count = await _statisticsService.GetBookingsCountByPeriodAsync(startDate, endDate);
            return Ok(count);
        }
    }
}
