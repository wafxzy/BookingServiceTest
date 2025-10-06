using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.DAL.Queries
{
    public static class StatisticsQueries
    {
        public const string GET_TOTAL_BOOKINGS=@"
                SELECT COUNT(*) 
                FROM Bookings 
                WHERE Status != 'Cancelled'";

        public const string GET_BOOKINGS_COUNT_BY_PERIOD= @"
                SELECT COUNT(*) 
                FROM Bookings 
                WHERE Status != 'Cancelled' 
                AND CheckInDate >= @StartDate 
                AND CheckOutDate <= @EndDate";

        public const string GET_TOTAL_REVENUE=@"
                SELECT COALESCE(SUM(TotalPrice), 0) 
                FROM Bookings 
                WHERE Status IN ('Confirmed', 'Completed')";

        public const string GET_BOOKINGS_BY_CITY= @"
                SELECT h.City, COUNT(b.Id) as Count
                FROM Bookings b
                INNER JOIN Rooms r ON b.RoomId = r.Id
                INNER JOIN Hotels h ON r.HotelId = h.Id
                WHERE b.Status != 'Cancelled'
                GROUP BY h.City
                ORDER BY Count DESC";

        public const string GET_BOOKINGS_BY_MONTH= @"
                SELECT 
                    MONTH(CheckInDate) as Month,
                    YEAR(CheckInDate) as Year,
                    COUNT(*) as Count
                FROM Bookings 
                WHERE YEAR(CheckInDate) = @Year 
                AND Status != 'Cancelled'
                GROUP BY MONTH(CheckInDate), YEAR(CheckInDate)
                ORDER BY Month";
    }
}
