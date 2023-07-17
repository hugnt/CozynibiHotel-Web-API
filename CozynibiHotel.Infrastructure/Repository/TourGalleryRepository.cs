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
    public class TourGalleryRepository : GenericRepository<TourGallery>, ITourGalleryRepository
    {
        public TourGalleryRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public bool SetDeletedAll(int tour_id)
        {
            try
            {
                var all = GetAll().Where(s => s.TourId == tour_id);
                foreach (var item in all)
                {
                    item.IsDeleted = true;
                    Update(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
