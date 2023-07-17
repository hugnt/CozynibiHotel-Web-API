using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tArticle")]
    public class Article : BaseModel
    {
        public string Title { get; set; }

        [Column("sub_title")]
        public string? SubTitle { get; set; }

        [Column("page_id")]
        public int? PageId { get; set; }

        public string? Content { get; set; }



    }
}
