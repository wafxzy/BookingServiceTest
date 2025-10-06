using BookingService.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(HotelServiceDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                await context.Database.MigrateAsync();
                await SeedRolesAsync(roleManager);
                await SeedAdminUserAsync(userManager);
                await SeedTestUsersAsync(userManager);
                await SeedHotelsAndRoomsAsync(context);
                await SeedBookingsAsync(context, userManager);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error seeding database: {ex.Message}", ex);
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Client" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        private static async Task SeedAdminUserAsync(UserManager<User> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync(SeedDataConstants.AdminUser.Email);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = SeedDataConstants.AdminUser.Email,
                    Email = SeedDataConstants.AdminUser.Email,
                    FirstName = SeedDataConstants.AdminUser.FirstName,
                    LastName = SeedDataConstants.AdminUser.LastName,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, SeedDataConstants.AdminUser.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private static async Task SeedTestUsersAsync(UserManager<User> userManager)
        {
            foreach (var userData in SeedDataConstants.TestUsers.Users)
            {
                var user = await userManager.FindByEmailAsync(userData.Email);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = userData.Email,
                        Email = userData.Email,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, userData.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Client");
                    }
                }
            }
        }

        private static async Task SeedHotelsAndRoomsAsync(HotelServiceDbContext context)
        {
            if (!await context.Hotels.AnyAsync())
            {
                var hotels = new List<Hotel>();

                foreach (var hotelData in SeedDataConstants.TestHotels.Hotels)
                {
                    hotels.Add(new Hotel
                    {
                        Name = hotelData.Name,
                        Address = hotelData.Address,
                        City = hotelData.City,
                        Description = hotelData.Description,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                await context.Hotels.AddRangeAsync(hotels);
                await context.SaveChangesAsync();
                var random = new Random();

                foreach (var hotel in hotels)
                {
                    var rooms = new List<Room>();
                    int roomCount = random.Next(5, 9);

                    for (int i = 1; i <= roomCount; i++)
                    {
                        var roomTypeData = SeedDataConstants.RoomTypes.Types[random.Next(SeedDataConstants.RoomTypes.Types.Length)];

                        var priceVariation = random.Next(-200, 301);
                        var finalPrice = Math.Max(500, roomTypeData.BasePrice + priceVariation);
                        var capacity = random.Next(roomTypeData.MinCapacity, roomTypeData.MaxCapacity + 1);

                        rooms.Add(new Room
                        {
                            HotelId = hotel.Id,
                            Number = $"{(i / 10) + 1}{i % 10:D2}",
                            Type = roomTypeData.Type,
                            PricePerNight = finalPrice,
                            Capacity = capacity,
                            Description = roomTypeData.Description,
                            IsAvailable = true,
                            CreatedAt = DateTime.UtcNow
                        });
                    }

                    await context.Rooms.AddRangeAsync(rooms);
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedBookingsAsync(HotelServiceDbContext context, UserManager<User> userManager)
        {
            if (!await context.Bookings.AnyAsync())
            {
                var clients = await userManager.GetUsersInRoleAsync("Client");
                var rooms = await context.Rooms.Take(10).ToListAsync();

                if (clients.Any() && rooms.Any())
                {
                    var bookings = new List<Booking>();
                    var random = new Random();
                    var baseDate = DateTime.Today.AddDays(-30);

                    for (int i = 0; i < 5; i++)
                    {
                        var client = clients[random.Next(clients.Count)];
                        var room = rooms[random.Next(rooms.Count)];

                        var checkInDate = baseDate.AddDays(random.Next(0, 60));
                        var stayDuration = random.Next(1, 8);
                        var checkOutDate = checkInDate.AddDays(stayDuration);

                        var totalPrice = room.PricePerNight * stayDuration;

                        var statuses = new[] { BookingStatus.Confirmed, BookingStatus.Completed, BookingStatus.Pending };
                        var status = checkOutDate < DateTime.Today ? BookingStatus.Completed : statuses[random.Next(statuses.Length)];

                        bookings.Add(new Booking
                        {
                            UserId = client.Id,
                            RoomId = room.Id,
                            CheckInDate = checkInDate,
                            CheckOutDate = checkOutDate,
                            TotalPrice = totalPrice,
                            Status = status,
                            CreatedAt = checkInDate.AddDays(-random.Next(1, 15))
                        });
                    }

                    await context.Bookings.AddRangeAsync(bookings);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
