using CozynibiHotel.Core.Dto;

namespace CozynibiHotel.API.Models
{
    public class RoomCategoryViewModel
    {
        public RoomCategoryDto RoomCategoryCreate { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
