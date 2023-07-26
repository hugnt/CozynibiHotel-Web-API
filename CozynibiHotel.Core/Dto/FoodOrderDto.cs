using CozynibiHotel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Dto
{
    public class FoodOrderDto : FoodOrder
    {
 
        public List<FoodOrderDetailsDto> FoodList { get; set; }
        public FoodOrderDto()
        {
            FoodList = new List<FoodOrderDetailsDto>();

        }
    }

}
