using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tGallery")]
    public class Gallery : BaseModel
    {
        public string? Image { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }

    }
}
