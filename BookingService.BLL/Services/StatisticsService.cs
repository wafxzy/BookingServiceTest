using BookingService.BLL.Services.Interfaces;
using BookingService.Common.DTOs;
using BookingService.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.BLL.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public async Task<StatisticsDto> GetStatisticsAsync()
        {
            var totalBookings = await _statisticsRepository.GetTotalBookingsAsync();
            var totalRevenue = await _statisticsRepository.GetTotalRevenueAsync();
            var bookingsByCity = await _statisticsRepository.GetBookingsByCityAsync();
            var monthlyBookings = await _statisticsRepository.GetBookingsByMonthAsync(DateTime.Now.Year);

            return new StatisticsDto
            {
                TotalBookings = totalBookings,
                TotalRevenue = totalRevenue,
                BookingsByCity = bookingsByCity.Select(x => new CityStatisticsDto
                {
                    City = x.City,
                    BookingsCount = x.Count
                }).ToList(),
                MonthlyBookings = monthlyBookings.Select(x => new MonthlyStatisticsDto
                {
                    Month = x.Month,
                    Year = x.Year,
                    BookingsCount = x.Count,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month)
                }).ToList()
            };
        }

        public async Task<int> GetBookingsCountByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _statisticsRepository.GetBookingsCountByPeriodAsync(startDate, endDate);
        }
    }
}
