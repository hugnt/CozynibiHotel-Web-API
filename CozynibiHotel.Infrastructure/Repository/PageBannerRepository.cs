using AutoMapper;
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
    public class PageBannerRepository : GenericRepository<PageBanner>, IPageBannerRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public PageBannerRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool UpdateStatus(int page_id, List<string> lstUpdateImage)
        {
            try
            {
                var lstPageBannerWithPageId = GetAll().Where(p => p.PageId == page_id);
                foreach (var ri in lstPageBannerWithPageId)
                {
                    if (lstUpdateImage.Contains(ri.Image))
                    {
                        ri.IsDeleted = false;
                    }
                    else
                    {
                        ri.IsDeleted = true;
                    }
                    Update(ri);
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
