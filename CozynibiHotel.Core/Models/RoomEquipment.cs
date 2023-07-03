
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tRoomEquipment")]
    public class RoomEquipment : BaseModel
    {
        [ForeignKey("tCategoryRoom")]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [ForeignKey("tEquipment")]
        [Column("equipment_id")]
        public int EquipmentId { get; set; }
    }
}
