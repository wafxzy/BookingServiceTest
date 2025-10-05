using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Common.DTOs
{
    public class StatisticsDto
    {
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<CityStatisticsDto> BookingsByCity { get; set; } = new();
        public List<MonthlyStatisticsDto> MonthlyBookings { get; set; } = new();
    }
}
