using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tFoodOrderDetails")]
    public class FoodOrderDetails : BaseModel
    {
        [Column("food_id")]
        public int FoodId { get; set; }
        [Column("foodorder_id")]
        public int FoodOrderId { get; set; }
        public int Number { get; set; }
    }
}
