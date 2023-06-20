
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tRoom")]
    public class Room : BaseModel
    {
        [ForeignKey("tCategoryRoom")]
        [Column("category_id")]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Hight { get; set; }

        [Column("bed_size")]
        public double? BedSize { get; set; }

        [Column("room_rate")]
        public double? RoomRate { get; set; }

        public string? Description { get; set; }
        
    }
}
