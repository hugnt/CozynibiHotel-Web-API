using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tTourPrice")]
    public class TourPrice : BaseModel
    {
        [Column("tour_id")]
        public int TourId { get; set; }
        public double? Price { get; set; }

        [Column("min_people")]
        public int? MinPeople { get; set; }

        [Column("max_people")]
        public int? MaxPeople { get; set; }
    }
}
