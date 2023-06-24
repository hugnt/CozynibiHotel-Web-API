
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tRoomImage")]
    public class RoomImage : BaseModel
    {
        [ForeignKey("tRoomCategory")]
        [Column("category_id")]
        public int CategoryId { get; set; }
        public string Image { get; set; }

    }
}
