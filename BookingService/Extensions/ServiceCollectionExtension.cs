using BookingService.BLL.Services;
using BookingService.BLL.Services.Interfaces;
using BookingService.DAL.Repositories;
using BookingService.DAL.Repositories.Interfaces;
using Dapper;

namespace BookingService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection Services)
        {

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            Services.AddScoped<IHotelRepository, HotelRepository>();
            Services.AddScoped<IRoomRepository, RoomRepository>();
            Services.AddScoped<IBookingRepository, BookingRepository>();
            Services.AddScoped<IStatisticsRepository, StatisticsRepository>();


            Services.AddScoped<IHotelService, HotelService>();
            Services.AddScoped<IRoomService, RoomService>();
            Services.AddScoped<IBookingService, BookingService.BLL.Services.BookingService>();
            Services.AddScoped<IAuthService, AuthService>();
            Services.AddScoped<IStatisticsService, StatisticsService>();

            return Services;
        }
    }
}
