
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tFood")]
    public class Food : BaseModel
    {
        [ForeignKey("tFoodCategory")]
        [Column("category_id")]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string? Image { get; set; }

        [Column("foodRate")]
        public double? FoodRate { get; set; }
        public string? Description { get; set; }
        
    }
}
