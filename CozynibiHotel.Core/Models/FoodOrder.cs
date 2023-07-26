using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tFoodOrder")]
    public class FoodOrder : BaseModel
    {
        public string FullName { get; set; }
        [Column("checkin_code")]
        public int? CheckInCode { get; set; }
        public string? Place { get; set; }
        public string? Note { get; set; }
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        public TimeSpan? EatingAt { get; set; }
        public int? Status { get; set; }


    }
}
