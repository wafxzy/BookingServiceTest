using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Common.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string HotelName { get; set; } = string.Empty;
    }
}
