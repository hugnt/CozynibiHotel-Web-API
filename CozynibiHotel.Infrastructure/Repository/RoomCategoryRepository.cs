﻿using AutoMapper;
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
            var queryRCE = from re in _dbContext.RoomEquipments
                           join e in _dbContext.Equipments
                           on re.EquipmentId equals e.Id
                           where re.IsDeleted == false
                           select new
                           {
                               CategoryId = re.CategoryId,
                               Name = e.Name
                           };

            var groupJoin = from rc in _dbContext.RoomCategories.ToList()
                            join ri in _dbContext.RoomImages.ToList() on rc.Id equals ri.CategoryId into rci
                            join re in queryRCE on rc.Id equals re.CategoryId into rcei
                            select new 
                            {
                                RoomCategoryObj = rc,
                                Images = rci,
                                Equipments = rcei
                              
                            };

            foreach (var item in groupJoin)
            {
                var roomCate = _mapper.Map<RoomCategoryDto>(item.RoomCategoryObj);
                foreach (var img in item.Images)
                {
                    if(img.IsDeleted == false) roomCate.Images.Add(img.Image);

                }
                foreach (var equip in item.Equipments)
                {
                    roomCate.Equipments.Add(equip.Name);
                }

                roomCategories.Add(roomCate);
            }
            return roomCategories;

        }


        public RoomCategoryDto GetByIdDto(int roomCategoryId)
        {
            return GetAll().FirstOrDefault(e => e.Id == roomCategoryId);
        }

        public ICollection<RoomCategoryDto> Search(string field, string keyWords)
        {
            if (keyWords == ""||keyWords == "*" || keyWords == null) return GetAll();
            field = field.ToLower();
            field = field.Substring(0, 1).ToUpper() + field.Substring(1);
            keyWords = keyWords.ToLower();
            if(field == "Isactive")
            {
     
                if (keyWords == "1" || keyWords.Contains("Active") || keyWords == "true")
                {
                    return GetAll().Where(e => e.IsActive==true).ToList();
                }
                else
                {
                    return GetAll().Where(e => e.IsActive == false).ToList();
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
                var selectedRecord = _dbContext.RoomCategories.Find(id);
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

    }
}
