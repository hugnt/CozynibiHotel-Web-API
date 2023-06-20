using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Dto
{
    public class RoomCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Hight { get; set; }
        public double BedSize { get; set; }
        public double RoomRate { get; set; }
        public string Description { get; set; }
    }
}
