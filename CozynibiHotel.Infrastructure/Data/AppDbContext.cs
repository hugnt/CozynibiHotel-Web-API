using CozynibiHotel.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<RefeshToken> RefeshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Language> Languages { get; set; }

        //Room
        public DbSet<RoomCategory> RoomCategories { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<RoomEquipment> RoomEquipments { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomGallery> RoomGalleries { get; set; }

        //Food
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Food> Foods { get; set; }

        //Tour Travel
        public DbSet<TourTravel> TourTravels { get; set; }
        public DbSet<TourExclusion> TourExclusions { get; set; }
        public DbSet<TourInclusion> TourInclusions { get; set; }
        public DbSet<TourSchedule> TourSchedules { get; set; }
        public DbSet<TourGallery> TourGalleries { get; set; }
        public DbSet<TourPrice> TourPrices { get; set; }
        public DbSet<Exclusion> Exclusions { get; set; }
        public DbSet<Inclusion> Inclusions { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<GalleryCategory> GalleryCategories { get; set; }

        //Service
        public DbSet<Service> Services { get; set; }

        //News
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<News> News { get; set; }

        //Page
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageBanner> PageBanners { get; set; }

        //Custommer
        public DbSet<Custommer> Custommers { get; set; }

        //Article
        public DbSet<Article> Articles { get; set; }

        //Information
        public DbSet<Information> Informations { get; set; }

        //Contact
        public DbSet<Contact> Contacts { get; set; }

        //Booking
        public DbSet<Booking> Bookings { get; set; }

    }
}
