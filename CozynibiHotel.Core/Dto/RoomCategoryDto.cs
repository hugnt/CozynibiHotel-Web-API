using CozynibiHotel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Dto
{
    public class RoomCategoryDto : RoomCategory
    {
        public List<string> Images { get; set; }
        public List<string> Equipments { get; set; }
        public RoomCategoryDto()
        {
            Images = new List<string>();
            Equipments = new List<string>();
        }
        
    }
}
