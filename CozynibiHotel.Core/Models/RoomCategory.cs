
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tRoomCategory")]
    public class RoomCategory : BaseModel
    {
        public string Name { get; set; }
        public double? Area { get; set; }
        public double? Hight { get; set; }

        [Column("bed_size")]
        public double? BedSize { get; set; }

        [Column("room_rate")]
        public double? RoomRate { get; set; }

        public string? Description { get; set; }
        
    }
}
