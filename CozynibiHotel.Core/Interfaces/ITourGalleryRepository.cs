using CozynibiHotel.Core.Dto;
using CozynibiHotel.Core.Models;
using HUG.CRUD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Interfaces
{
    public interface ITourGalleryRepository : IGenericRepository<TourGallery>
    {
        bool SetDeletedAll(int tour_id);
    }
}
