using BookingService.DAL.Queries;
using BookingService.DAL.Repositories.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.DAL.Repositories
{

    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly string _connectionString;

        public StatisticsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<int> GetTotalBookingsAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            return await connection.QuerySingleAsync<int>(StatisticsQueries.GET_TOTAL_BOOKINGS);
        }

        public async Task<int> GetBookingsCountByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = new MySqlConnection(_connectionString);

            return await connection.QuerySingleAsync<int>(StatisticsQueries.GET_BOOKINGS_COUNT_BY_PERIOD, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            return await connection.QuerySingleAsync<decimal>(StatisticsQueries.GET_TOTAL_REVENUE);
        }

        public async Task<IEnumerable<(string City, int Count)>> GetBookingsByCityAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            var result = await connection.QueryAsync<(string City, int Count)>(StatisticsQueries.GET_BOOKINGS_BY_CITY);
            return result;
        }

        public async Task<IEnumerable<(int Month, int Year, int Count)>> GetBookingsByMonthAsync(int year)
        {
            using var connection = new MySqlConnection(_connectionString);

            var result = await connection.QueryAsync<(int Month, int Year, int Count)>(StatisticsQueries.GET_BOOKINGS_BY_MONTH, new { Year = year });
            return result;
        }
    }
}
