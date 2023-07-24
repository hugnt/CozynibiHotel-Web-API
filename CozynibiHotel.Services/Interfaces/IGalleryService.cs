using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IGalleryService
    {
        IEnumerable<GalleryCategoryDto> GetGalleryCategories();
        GalleryCategoryDto GetGalleryCategory(int galleryCategoryId);
        IEnumerable<GalleryDto> GetGalleries();
        IEnumerable<GalleryDto> SearchGalleries(string field, string keyWords);
        GalleryDto GetGallery(int galleryId);
        ResponseModel CreateGallery(GalleryDto galleryCreate);
        ResponseModel UpdateGallery(int galleryId, GalleryDto updatedGallery);
        ResponseModel UpdateGallery(int galleryId, bool isDelete);
        ResponseModel DeleteGallery(int galleryCategoryId);

    }
}
