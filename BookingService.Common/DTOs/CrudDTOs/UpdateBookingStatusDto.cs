using BookingService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Common.DTOs.CrudDTOs
{
    public class UpdateBookingStatusDto
    {
        public BookingStatus Status { get; set; }
    }
}
