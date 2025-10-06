using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.DAL.Data
{

    public static class SeedDataConstants
    {
        public static class AdminUser
        {
            public const string Email = "admin@g.com";
            public const string Password = "Admin123!";
            public const string FirstName = "amin";
            public const string LastName = "admon";
        }

        public static class TestUsers
        {
            public static readonly (string Email, string Password, string FirstName, string LastName)[] Users =
            {
                ("client1@t.com", "Client123!", "yo", "asd"),
                ("client2@t.com", "Client123!", "aha", "ajaj"),
            };
        }

        public static class TestHotels
        {
            public static readonly (string Name, string Address, string City, string Description)[] Hotels =
            {
                (
                    "Grand Hotel Kyiv",
                    "jasjksd 1",
                    "Kyiv",
                    "dxdsxsfdjdfjdkd."
                ),
                (
                    "Odessa",
                    "sshjssjs",
                    "Odessa",
                    "jadjkskssjsjzsjzzsjzsmjs."
                ),
                (
                    "Kharkiv eeeee",
                    "center city ",
                    "Kharkiv",
                    "whjwsjhsdjhdhjdddidd."
                )

            };
        }

            public static class RoomTypes
            {
                public static readonly (string Type, int BasePrice, int MinCapacity, int MaxCapacity, string Description)[] Types =
                {
                ("Stndrt", 1000, 1, 2, "des criptions abt hotel nomer"),
                ("Plus", 1500, 1, 2, "des criptions abt hotel nomer"),
                ("Family", 1800, 3, 4, "des criptions abt hotel nomer"),
                ("Bzns", 2000, 1, 2, "des criptions abt hotel nomer"),
                ("Lux", 2500, 2, 2, "des criptions abt hotel nomer"),
                ("Superlux", 5000, 2, 4, "des criptions abt hotel nomer")
            };
            }
        }
    }

