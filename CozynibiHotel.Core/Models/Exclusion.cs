using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tExclusion")]
    public class Exclusion : BaseModel
    {
        public string Name { get; set; }

    }
}
