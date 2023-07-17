using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tNewsCategory")]
    public class NewsCategory : BaseModel
    {
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
    }
}
