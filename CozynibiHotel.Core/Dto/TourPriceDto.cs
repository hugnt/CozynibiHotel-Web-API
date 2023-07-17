using CozynibiHotel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Dto
{
    public class TourPriceDto
    {
        public double Price { get; set; }
        public int MinPeople { get; set; }
        public int MaxPeople { get; set; }
    }
}
