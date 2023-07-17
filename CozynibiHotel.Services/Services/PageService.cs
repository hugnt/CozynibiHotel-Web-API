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
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;
        private readonly IPageBannerRepository _pageBannerRepository;

        public PageService(IPageRepository pageRepository, 
                                   IMapper mapper,
                                   IPageBannerRepository pageBannerRepository
                                   )
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
            _pageBannerRepository = pageBannerRepository;
        }

        public PageDto GetPage(int pageId)
        {
            if (!_pageRepository.IsExists(pageId)) return null;
            var page = _pageRepository.GetByIdDto(pageId);
            return page;
        }
        public IEnumerable<PageDto> GetPages()
        {
            return _pageRepository.GetAll();
        }
        public ResponseModel CreatePage(PageDto pageCreate)
        {
            if (pageCreate.CreatedBy == 0) pageCreate.CreatedBy = 1;
            if (pageCreate.UpdatedBy == 0) pageCreate.UpdatedBy = 1;
            pageCreate.CreatedAt = DateTime.Now;
            pageCreate.IsActive = false;
            pageCreate.IsDeleted = false;
            var pages = _pageRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == pageCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (pages != null)
            {
                return new ResponseModel(422, "Page already exists");
            }

            
            var pageMap = _mapper.Map<Page>(pageCreate);
            pageMap.CreatedAt = DateTime.Now;


            if (!_pageRepository.Create(pageMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            foreach(var img in pageCreate.Images)
            {
                var pageBanner = new PageBanner()
                {
                    PageId = pageMap.Id,
                    Image = img,
                    CreatedBy = pageCreate.CreatedBy,
                    UpdatedBy = pageCreate.UpdatedBy,
                    IsDeleted = false
                };
                var checkImgExist = _pageBannerRepository.GetAll().Any(img => 
                                                                    img.PageId== pageBanner.PageId &&
                                                                    img.Image == pageBanner.Image);
                if (checkImgExist) continue;
                if (!_pageBannerRepository.Create(pageBanner))
                {
                    return new ResponseModel(500, "Something went wrong while saving images");
                }
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdatePage(int pageId, PageDto updatedPage)
        {
            if (updatedPage.CreatedBy == 0) updatedPage.CreatedBy = 1;
            if (updatedPage.UpdatedBy == 0) updatedPage.UpdatedBy = 1;
            updatedPage.UpdatedAt = DateTime.Now;

            if (!_pageRepository.IsExists(pageId)) return new ResponseModel(404,"Not found");
            var pageMap = _mapper.Map<Page>(updatedPage);
            if (!_pageRepository.Update(pageMap))
            {
                return new ResponseModel(500, "Something went wrong updating page");
            }

            //Images
            foreach (var img in updatedPage.Images)
            {
                var pageBanner = new PageBanner()
                {
                    PageId = pageId,
                    Image = img,
                    CreatedBy = updatedPage.CreatedBy,
                    UpdatedBy = updatedPage.UpdatedBy,
                    IsDeleted = false
                };
                var checkImgExist = _pageBannerRepository.GetAll().Any(img =>
                                                                    img.PageId == pageBanner.PageId &&
                                                                    img.Image == pageBanner.Image);
                if (checkImgExist) continue;
                if (!_pageBannerRepository.Create(pageBanner))
                {
                    return new ResponseModel(500, "Something went wrong while saving images");
                }
            }
            if (!_pageBannerRepository.UpdateStatus(pageId, updatedPage.Images))
            {
                return new ResponseModel(500, "Something went wrong updating status of images page");
            }

            return new ResponseModel(204, "");
        }


        public ResponseModel UpdatePage(int pageId, bool isDelete)
        {
            if(!_pageRepository.SetDelete(pageId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete page");
            }
            return new ResponseModel(204, "");
        }

        public ResponseModel DeletePage(int pageId)
        {
            if (!_pageRepository.IsExists(pageId)) return new ResponseModel(404, "Not found");
            var pageToDelete = _pageRepository.GetById(pageId);
            if (!_pageRepository.Delete(pageToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting page");
            }

            return new ResponseModel(204, "");
        }

        public IEnumerable<PageDto> SearchPages(string field, string keyWords)
        {
            var res = _pageRepository.Search(field, keyWords);
            return res;
        }
    }
}
