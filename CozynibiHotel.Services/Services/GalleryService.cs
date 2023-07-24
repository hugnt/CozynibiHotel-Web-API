using AutoMapper;
using CozynibiHotel.Core.Dto;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Services.Interfaces;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IGalleryRepository _galleryRepository;
        private readonly IGalleryCategoryRepository _galleryCategoryRepository;
        private readonly IMapper _mapper;

        public GalleryService(IGalleryRepository galleryRepository,
                              IGalleryCategoryRepository galleryCategoryRepository,
                              IMapper mapper)
        {
            _galleryRepository = galleryRepository;
            _galleryCategoryRepository = galleryCategoryRepository;
            _mapper = mapper;
        }

        public GalleryDto GetGallery(int galleryId)
        {
            if (!_galleryRepository.IsExists(galleryId)) return null;
            var galleryMap = _mapper.Map<GalleryDto>(_galleryRepository.GetById(galleryId));
            return galleryMap;
        }

        public IEnumerable<GalleryDto> GetGalleries()
        {
            var galleriesMap = _mapper.Map<List<GalleryDto>>(_galleryRepository.GetAll());
            return galleriesMap;
        }
        public ResponseModel CreateGallery(GalleryDto galleryCreate)
        {
            if (galleryCreate.CreatedBy == 0) galleryCreate.CreatedBy = 1;
            if (galleryCreate.UpdatedBy == 0) galleryCreate.UpdatedBy = 1;
            galleryCreate.CreatedAt = DateTime.Now;
            galleryCreate.IsActive = false;
            galleryCreate.IsDeleted = false;
            var galleries = _galleryRepository.GetAll()
                            .Where(l => l.Image == galleryCreate.Image)
                            .FirstOrDefault();
            if (galleries != null)
            {
                return new ResponseModel(422, "Gallery already exists");
            }


            var galleryMap = _mapper.Map<Gallery>(galleryCreate);
            galleryMap.CreatedAt = DateTime.Now;


            if (!_galleryRepository.Create(galleryMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateGallery(int galleryId, GalleryDto updatedGallery)
        {
            if (updatedGallery.CreatedBy == 0) updatedGallery.CreatedBy = 1;
            if (updatedGallery.UpdatedBy == 0) updatedGallery.UpdatedBy = 1;
            updatedGallery.UpdatedAt = DateTime.Now;

            if (!_galleryRepository.IsExists(galleryId)) return new ResponseModel(404, "Not found");
            var galleryMap = _mapper.Map<Gallery>(updatedGallery);
            if (!_galleryRepository.Update(galleryMap))
            {
                return new ResponseModel(500, "Something went wrong updating gallery");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteGallery(int galleryId)
        {
            if (!_galleryRepository.IsExists(galleryId)) return new ResponseModel(404, "Not found");
            var galleryToDelete = _galleryRepository.GetById(galleryId);
            if (!_galleryRepository.Delete(galleryToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting gallery");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<GalleryDto> SearchGalleries(string field, string keyWords)
        {
            var res = _galleryRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateGallery(int galleryId, bool isDelete)
        {
            if (!_galleryRepository.SetDelete(galleryId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete gallery");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<GalleryCategoryDto> GetGalleryCategories()
        {
            var galleriesMap = _mapper.Map<List<GalleryCategoryDto>>(_galleryCategoryRepository.GetAll());
            return galleriesMap;
        }

        public GalleryCategoryDto GetGalleryCategory(int galleryCategoryId)
        {
            if (!_galleryCategoryRepository.IsExists(galleryCategoryId)) return null;
            var galleryMap = _mapper.Map<GalleryCategoryDto>(_galleryCategoryRepository.GetById(galleryCategoryId));
            return galleryMap;
        }
    }
}
