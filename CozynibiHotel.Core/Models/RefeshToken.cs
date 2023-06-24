
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tRefeshToken")]
    public class RefeshToken : BaseModel
    {
        [ForeignKey("tAccount")]
        [Column("account_id")]
        public int AccountId { get; set; }
        public string? Token { get; set; }

        [Column("jwt_id")]
        public string? JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? IssuedAt { get; set; }
        public DateTime? ExpireAt { get; set; }

    }
}
