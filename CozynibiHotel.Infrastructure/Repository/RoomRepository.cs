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
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public RoomRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ICollection<RoomDto> GetAll()
        {
            var rooms = new List<RoomDto>();
            var groupJoin = from rc in _dbContext.Rooms.ToList()
                            join ri in _dbContext.RoomGalleries.ToList() on rc.Id equals ri.RoomId into rci
                            select new
                            {
                                RoomObj = rc,
                                Images = rci,

                            };

            foreach (var item in groupJoin)
            {
                var roomCate = _mapper.Map<RoomDto>(item.RoomObj);
                foreach (var img in item.Images)
                {
                    if (img.IsDeleted == false) roomCate.Images.Add(img.Image);

                }

                rooms.Add(roomCate);
            }
            return rooms;
        }
        public RoomDto GetByIdDto(int roomId)
        {
            return GetAll().FirstOrDefault(e => e.Id == roomId);
        }

        public bool SetDelete(int id, bool isDelete)
        {
            try
            {
                var selectedRecord = _dbContext.Rooms.Find(id);
                if (selectedRecord != null)
                {
                    selectedRecord.IsDeleted = isDelete;
                    selectedRecord.IsActive = false;
                    Update(selectedRecord);
                }

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        public ICollection<RoomDto> Search(string field, string keyWords)
        {
            if (keyWords == "" || keyWords == "*" || keyWords == null) return GetAll();
            field = field.ToLower();
            field = field.Substring(0, 1).ToUpper() + field.Substring(1);
            keyWords = keyWords.ToLower();
            if (field == "Isactive")
            {

                if (keyWords == "1" || keyWords.Contains("Active") || keyWords == "true")
                {
                    return GetAll().Where(e => e.IsActive == true).ToList();
                }
                else
                {
                    return GetAll().Where(e => e.IsActive == false).ToList();
                }

            }
            if(field == "Category")
            {
                var lstCateMatch = _dbContext.RoomCategories.ToList().Where(c => c.Name.ToLower().Contains(keyWords)).Select(c => c.Id);
                return GetAll().Where(r => lstCateMatch.Contains(r.CategoryId)).ToList();

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
    }
}
