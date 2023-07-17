using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tCustommer")]
    public class Custommer : BaseModel
    {
        public string FullName { get; set; }
        public string? Address { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Comment { get; set; }

        [Column("room_id")]
        public int? RoomId { get; set; }

        public string? Image { get; set; }
        public string? Country { get; set; }


    }
}
