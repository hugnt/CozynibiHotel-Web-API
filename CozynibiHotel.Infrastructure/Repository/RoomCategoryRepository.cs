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
    public class RoomCategoryRepository : GenericRepository<RoomCategory>, IRoomCategoryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public RoomCategoryRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ICollection<RoomCategoryDto> GetAll()
        {
            var roomCategories = new List<RoomCategoryDto>();
            var groupJoin = from rc in _dbContext.RoomCategories.ToList()
                            join ri in _dbContext.RoomImages.ToList()
                            on rc.Id equals ri.CategoryId into rci
                            select new 
                            {
                                RoomCategoryObj = rc,
                                Images = rci
                            };

            foreach (var item in groupJoin)
            {
                var roomCate = _mapper.Map<RoomCategoryDto>(item.RoomCategoryObj);
                foreach (var img in item.Images)
                {
                    roomCate.Images.Add(img.Image);       
                }
                roomCategories.Add(roomCate);
            }
            return roomCategories;

        }

    }
}
