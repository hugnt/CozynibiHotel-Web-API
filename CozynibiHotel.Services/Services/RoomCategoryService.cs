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
    public class RoomCategoryService : IRoomCategoryService
    {
        private readonly IRoomCategoryRepository _roomCategoryRepository;
        private readonly IMapper _mapper;

        public RoomCategoryService(IRoomCategoryRepository roomCategoryRepository, IMapper mapper)
        {
            _roomCategoryRepository = roomCategoryRepository;
            _mapper = mapper;
        }

        public RoomCategoryDto GetRoomCategory(int roomCategoryId)
        {
            if (!_roomCategoryRepository.IsExists(roomCategoryId)) return null;
            var roomCategory = _roomCategoryRepository.GetById(roomCategoryId);
            return roomCategory;
        }
        public IEnumerable<RoomCategoryDto> GetRoomCategories()
        {
            return _roomCategoryRepository.GetAll();
        }
        public ResponseModel CreateRoomCategory(RoomCategoryDto roomCategoryCreate)
        {
            var roomCategories = _roomCategoryRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == roomCategoryCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (roomCategories != null)
            {
                return new ResponseModel(422, "RoomCategory already exists");
            }

            var roomCategoryMap = _mapper.Map<RoomCategory>(roomCategoryCreate);

            if (!_roomCategoryRepository.Create(roomCategoryMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateRoomCategory(int roomCategoryId, RoomCategoryDto updatedRoomCategory)
        {
            if (!_roomCategoryRepository.IsExists(roomCategoryId)) return new ResponseModel(404,"Not found");
            var roomCategoryMap = _mapper.Map<RoomCategory>(updatedRoomCategory);
            if (!_roomCategoryRepository.Update(roomCategoryMap))
            {
                return new ResponseModel(500, "Something went wrong updating roomCategory");
            }
            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteRoomCategory(int roomCategoryId)
        {
            if (!_roomCategoryRepository.IsExists(roomCategoryId)) return new ResponseModel(404, "Not found");
            var roomCategoryToDelete = _roomCategoryRepository.GetById(roomCategoryId);
            if (!_roomCategoryRepository.Delete(roomCategoryToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting roomCategory");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<RoomCategoryDto> SearchRoomCategories(string field, string keyWords)
        {
            var res = _roomCategoryRepository.Search(field, keyWords);
            return res;
        }
    }
}
