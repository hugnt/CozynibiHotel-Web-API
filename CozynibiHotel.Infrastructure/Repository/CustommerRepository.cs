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
    public class CustommerRepository : GenericRepository<Custommer>, ICustommerRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public CustommerRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool SetDelete(int id, bool isDelete)
        {
            try
            {
                var selectedRecord = _dbContext.Custommers.Find(id);
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


        public ICollection<CustommerDto> Search(string field, string keyWords)
        {
            var all = _mapper.Map<List<CustommerDto>>(GetAll());
            if (keyWords == "" || keyWords == "*" || keyWords == null) return all;
            field = field.ToLower();
            field = field.Substring(0, 1).ToUpper() + field.Substring(1);
            keyWords = keyWords.ToLower();
            if (field == "Isactive")
            {

                if (keyWords == "1" || keyWords.Contains("active") || keyWords == "true")
                {
                    return all.Where(e => e.IsActive == true).ToList();
                }
                else
                {
                    return all.Where(e => e.IsActive == false).ToList();
                }

            }
            if(field == "Category")
            {
                var lstRoomMatch = _dbContext.Rooms.ToList().Where(c => c.Name.ToLower().Contains(keyWords)).Select(c => c.Id);
                return all.Where(r => lstRoomMatch.All(x => x == r.RoomId)).ToList();

            }

            var res = all
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
