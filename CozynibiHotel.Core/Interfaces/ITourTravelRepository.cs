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
    public interface ITourTravelRepository : IGenericRepository<TourTravel>
    {
        ICollection<TourTravelDto> GetAll();
        TourTravelDto GetByIdDto(int tourTravelId);
        ICollection<TourTravelDto> Search(string field, string keyWords);
        bool SetDelete(int id, bool isDelete);
    }
}
