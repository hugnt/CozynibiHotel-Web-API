using CozynibiHotel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Dto
{
    public class TourScheduleDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Content { get; set; }
    }
}
