using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.DAL.Repositories.Interfaces
{
    public interface IStatisticsRepository
    {
    Task<int> GetTotalBookingsAsync();
    Task<int> GetBookingsCountByPeriodAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalRevenueAsync();
    Task<IEnumerable<(string City, int Count)>> GetBookingsByCityAsync();
    Task<IEnumerable<(int Month, int Year, int Count)>> GetBookingsByMonthAsync(int year);
    }
}
