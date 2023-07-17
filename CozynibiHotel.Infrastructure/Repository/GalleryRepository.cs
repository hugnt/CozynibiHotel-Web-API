using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Infrastructure.Data;
using HUG.CRUD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Infrastructure.Repository
{
    public class GalleryRepository : GenericRepository<Gallery>, IGalleryRepository
    {
        public GalleryRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

    }
}
