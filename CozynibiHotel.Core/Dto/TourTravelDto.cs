using CozynibiHotel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Dto
{
    public class TourTravelDto : TourTravel
    {
        public List<string> TourExclusions { get; set; }
        public List<string> TourInclusions { get; set; }
        public List<TourPriceDto> TourPrices { get; set; }
        public List<string> TourGalleries { get; set; }
        public List<TourScheduleDto> TourSchedules { get; set; }
        public TourTravelDto()
        {
            TourExclusions = new List<string>();
            TourInclusions = new List<string>();
            TourPrices = new List<TourPriceDto>();
            TourGalleries = new List<string>();
            TourSchedules = new List<TourScheduleDto>();

        }
    }

}
