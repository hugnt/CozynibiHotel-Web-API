using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface ITourTravelService
    {
        IEnumerable<TourTravelDto> GetTourTravels();
        IEnumerable<TourTravelDto> SearchTourTravels(string field, string keyWords);
        TourTravelDto GetTourTravel(int tourTravelId);
        ResponseModel CreateTourTravel(TourTravelDto tourTravelCreate);
        ResponseModel UpdateTourTravel(int tourTravelId, TourTravelDto updatedTourTravel);
        ResponseModel UpdateTourTravel(int tourTravelId, bool isDelete);
        ResponseModel DeleteTourTravel(int tourTravelId);

    }
}
