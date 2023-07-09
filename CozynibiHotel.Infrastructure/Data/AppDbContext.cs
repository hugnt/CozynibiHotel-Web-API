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
        public DbSet<RoomCategory> RoomCategories { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<RoomEquipment> RoomEquipments { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomGallery> RoomGalleries { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Food> Foods { get; set; }

    }
}
