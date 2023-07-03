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
        private readonly IRoomImageRepository _roomImageRepository;
        private readonly IRoomEquipmentRepository _roomEquipmentRepository;
        private readonly IEquipmentRepository _equipmentRepository;

        public RoomCategoryService(IRoomCategoryRepository roomCategoryRepository, 
                                   IMapper mapper,
                                   IRoomImageRepository roomImageRepository,
                                   IRoomEquipmentRepository roomEquipmentRepository,
                                   IEquipmentRepository equipmentRepository
                                   )
        {
            _roomCategoryRepository = roomCategoryRepository;
            _mapper = mapper;
            _roomImageRepository = roomImageRepository;
            _roomEquipmentRepository = roomEquipmentRepository;
            _equipmentRepository = equipmentRepository;
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
            if (roomCategoryCreate.CreatedBy == 0) roomCategoryCreate.CreatedBy = 1;
            if (roomCategoryCreate.UpdatedBy == 0) roomCategoryCreate.UpdatedBy = 1;
            var roomCategories = _roomCategoryRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == roomCategoryCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (roomCategories != null)
            {
                return new ResponseModel(422, "RoomCategory already exists");
            }

            
            var roomCategoryMap = _mapper.Map<RoomCategory>(roomCategoryCreate);
            roomCategoryMap.CreatedAt = DateTime.Now;


            if (!_roomCategoryRepository.Create(roomCategoryMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            foreach(var img in roomCategoryCreate.Images)
            {
                var roomImage = new RoomImage()
                {
                    CategoryId = roomCategoryMap.Id,
                    Image = img,
                    CreatedBy = roomCategoryCreate.CreatedBy,
                    UpdatedBy = roomCategoryCreate.UpdatedBy,
                };
                var checkImgExist = _roomImageRepository.GetAll().Any(img => 
                                                                    img.CategoryId== roomImage.CategoryId &&
                                                                    img.Image == roomImage.Image);
                if (checkImgExist) continue;
                if (!_roomImageRepository.Create(roomImage))
                {
                    return new ResponseModel(500, "Something went wrong while saving images");
                }
            }

            foreach (var equip in roomCategoryCreate.Equipments)
            {
                //Create || Checking exist
                var checkEquipExist = _equipmentRepository.GetAll().FirstOrDefault(e =>
                                      e.Name.Trim().ToLower() == equip.Trim().ToLower());
                if (checkEquipExist == null)
                {
                    var newEquip = new Equipment()
                    {
                        Name = equip,
                        CreatedBy = roomCategoryCreate.CreatedBy,
                        UpdatedBy = roomCategoryCreate.UpdatedBy
                    };
                    if (!_equipmentRepository.Create(newEquip))
                    {
                        return new ResponseModel(500, "Something went wrong while adding new equipment");
                    }
                    checkEquipExist = newEquip;
                }

              //Create relation ship
              var roomEquipment = new RoomEquipment()
                {
                    CategoryId = roomCategoryMap.Id,
                    EquipmentId = checkEquipExist.Id,
                    CreatedBy = roomCategoryCreate.CreatedBy,
                    UpdatedBy = roomCategoryCreate.UpdatedBy
                };
                var checkRoomEquipExist = _roomEquipmentRepository.GetAll().Any(equip =>
                                                                    equip.CategoryId == roomEquipment.CategoryId &&
                                                                    equip.EquipmentId == roomEquipment.Id);
                if (checkRoomEquipExist) continue;
                if (!_roomEquipmentRepository.Create(roomEquipment))
                {
                    return new ResponseModel(500, "Something went wrong while saving images");
                }
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
            if(!_roomImageRepository.UpdateStatus(roomCategoryId, updatedRoomCategory.Images))
            {
                return new ResponseModel(500, "Something went wrong updating status of images roomCategory");
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
