
using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tFoodCategory")]
    public class FoodCategory : BaseModel
    {
        public string Name { get; set; }
        public string? Image { get; set; }
        public double? FoodRate { get; set; }
        public string? Description { get; set; }

    }
}
