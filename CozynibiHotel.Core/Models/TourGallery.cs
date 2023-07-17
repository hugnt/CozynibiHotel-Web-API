using HUG.CRUD.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Models
{
    [Table("tTourGallery")]
    public class TourGallery : BaseModel
    {
        [Column("tour_id")]
        public int TourId { get; set; }

        [Column("gallery_id")]
        public int GalleryId { get; set; }

    }
}
