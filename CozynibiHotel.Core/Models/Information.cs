using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tInformation")]
    public class Information : BaseModel
    {
        public string? Name { get; set; }
        public string? Logo { get; set; }
        public string? Slogan { get; set; }
        public string? Address { get; set; }
        public string? Site { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Facebook { get; set; }
        public string? Youtube { get; set; }
        public string? Instar { get; set; }
        public string? Twister { get; set; }
        public string? Description { get; set; }
        public string? FacebookLink { get; set; }
        public string? YoutubeLink { get; set; }
        public string? InstarLink { get; set; }
        public string? TwisterLink { get; set; }
        public string? GoogleMapLink { get; set; }


    }
}
