
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tPageBanner")]
    public class PageBanner : BaseModel
    {
        [ForeignKey("tPage")]
        [Column("page_id")]
        public int PageId { get; set; }
        public string? Image { get; set; }

    }
}
