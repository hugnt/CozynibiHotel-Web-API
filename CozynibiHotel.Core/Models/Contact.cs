using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tContact")]
    public class Contact : BaseModel
    {
        public string FullName { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
        public string? Comments { get; set; }


    }
}
