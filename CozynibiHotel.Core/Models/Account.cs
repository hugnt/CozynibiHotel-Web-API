
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tAccount")]
    public class Account : BaseModel
    {
        [Column("role_id")]
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }

    }
}
