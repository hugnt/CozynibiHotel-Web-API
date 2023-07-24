using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tBooking")]
    public class Booking : BaseModel
    {
        public string FullName { get; set; }
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        [Column("checkin")]
        public DateTime? CheckIn { get; set; }
        [Column("checkout")]
        public DateTime? CheckOut { get; set; }
        public string? Content { get; set; }
        [Column("checkin_code")]
        public int? CheckInCode { get; set; }
        public bool? IsSuccess { get; set; }
        public bool? IsConfirm { get; set; }
    }
}
