using BookingService.Common.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.DAL.Data
{

    public class HotelServiceDbContext : IdentityDbContext<User>
    {
        public HotelServiceDbContext(DbContextOptions<HotelServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        //i know that i can use those required fields in the entities but i want to show you 
        //that i know that i can add those configurations here as well
        //because most of my projects used fluent migrator where i have to add those configurations 
        //in sql scripts and use dapper to execute those scripts

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Name).HasMaxLength(200).IsRequired();
                entity.Property(h => h.Address).HasMaxLength(500).IsRequired();
                entity.Property(h => h.City).HasMaxLength(100).IsRequired();
                entity.Property(h => h.Description).HasMaxLength(1000);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Number).HasMaxLength(50).IsRequired();
                entity.Property(r => r.Type).HasMaxLength(100).IsRequired();
                entity.Property(r => r.PricePerNight).HasPrecision(18, 2);
                entity.Property(r => r.Description).HasMaxLength(1000);

                entity.HasOne(r => r.Hotel)
                    .WithMany(h => h.Rooms)
                    .HasForeignKey(r => r.HotelId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.TotalPrice).HasPrecision(18, 2);
                entity.Property(b => b.Status).HasConversion<string>();

                entity.HasOne(b => b.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Room)
                    .WithMany(r => r.Bookings)
                    .HasForeignKey(b => b.RoomId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
