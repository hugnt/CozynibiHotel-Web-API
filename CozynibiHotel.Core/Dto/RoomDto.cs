﻿using CozynibiHotel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Dto
{
    public class RoomDto : Room
    {
        public List<string> Images { get; set; }
        public RoomDto()
        {
            Images = new List<string>();
        }
    }
}
