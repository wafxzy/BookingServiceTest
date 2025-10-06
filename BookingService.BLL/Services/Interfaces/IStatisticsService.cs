using BookingService.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.BLL.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> GetStatisticsAsync();
        Task<int> GetBookingsCountByPeriodAsync(DateTime startDate, DateTime endDate);
    }
}
