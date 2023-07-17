
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tNews")]
    public class News : BaseModel
    {
        [ForeignKey("tNewsCategory")]
        [Column("category_id")]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Src { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        
    }
}
