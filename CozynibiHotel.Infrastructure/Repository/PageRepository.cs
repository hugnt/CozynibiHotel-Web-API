using AutoMapper;
using CozynibiHotel.Core.Dto;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Infrastructure.Data;
using HUG.CRUD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Infrastructure.Repository
{
    public class PageRepository : GenericRepository<Page>, IPageRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public PageRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ICollection<PageDto> GetAll()
        {
            var pages = new List<PageDto>();

            var groupJoin = from p in _dbContext.Pages.ToList()
                            join pi in _dbContext.PageBanners.ToList() on p.Id equals pi.PageId into ppi
                            select new 
                            {
                                Page = p,
                                Images = ppi,
                              
                            };

            foreach (var item in groupJoin)
            {
                var page = _mapper.Map<PageDto>(item.Page);
                foreach (var img in item.Images)
                {
                    if(img.IsDeleted == false) page.Images.Add(img.Image);

                }

                pages.Add(page);
            }
            return pages;

        }


        public PageDto GetByIdDto(int pageId)
        {
            return GetAll().FirstOrDefault(e => e.Id == pageId);
        }

        public ICollection<PageDto> Search(string field, string keyWords)
        {
            if (keyWords == ""||keyWords == "*" || keyWords == null) return GetAll();
            field = field.ToLower();
            field = field.Substring(0, 1).ToUpper() + field.Substring(1);
            keyWords = keyWords.ToLower();
            if(field == "Isactive")
            {
     
                if (keyWords == "1" || keyWords.Contains("active") || keyWords == "true")
                {
                    return GetAll().Where(e => e.IsActive==true).ToList();
                }
                else
                {
                    return GetAll().Where(e => e.IsActive == false).ToList();
                }
                
            }

            if (field == "Ismenuactive")
            {

                if (keyWords == "1" || keyWords.Contains("active") || keyWords == "true" || keyWords.Contains("yes") || keyWords.Contains("Yes"))
                {
                    return GetAll().Where(e => e.IsMenuActive == true).ToList();
                }
                else
                {
                    return GetAll().Where(e => e.IsMenuActive == false).ToList();
                }

            }

            var res = GetAll()
            .Where(e => e.GetType().GetProperty(field)?
            .GetValue(e)?
            .ToString()?
            .ToLower()
            .Contains(keyWords) ?? false)
            .ToList();
            if (res.Count() > 0) return res;

            return null;
        }

        public bool SetDelete(int id, bool isDelete)
        {
            try
            {
                var selectedRecord = _dbContext.Pages.Find(id);
                if (selectedRecord != null)
                {
                    selectedRecord.IsDeleted = isDelete;
                    selectedRecord.IsActive = false;
                    selectedRecord.IsMenuActive = false;
                    Update(selectedRecord);
                }
                
            }
            catch (Exception)
            {

                return false;
            }
            return true;
            
        }

    }
}
