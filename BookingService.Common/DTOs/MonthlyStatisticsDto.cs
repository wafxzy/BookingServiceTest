using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Common.DTOs
{
    public class MonthlyStatisticsDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int BookingsCount { get; set; }
        public string MonthName { get; set; } = string.Empty;
    }
}
