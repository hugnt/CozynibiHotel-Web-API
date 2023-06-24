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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<RoomCategory> RoomCategories { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }
}
