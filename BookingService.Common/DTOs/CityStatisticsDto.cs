using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Common.DTOs
{
    public class CityStatisticsDto
    {
        public string City { get; set; } = string.Empty;
        public int BookingsCount { get; set; }
    }

}
