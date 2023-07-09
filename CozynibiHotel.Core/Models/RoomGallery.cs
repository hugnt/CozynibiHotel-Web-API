
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tRoomGallery")]
    public class RoomGallery : BaseModel
    {
        [ForeignKey("tRoom")]
        [Column("room_id")]
        public int RoomId { get; set; }
        public string Image { get; set; }

    }
}
