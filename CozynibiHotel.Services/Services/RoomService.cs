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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomGalleryRepository _roomGalleryRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository,
                            IRoomGalleryRepository roomGalleryRepository,
                            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _roomGalleryRepository = roomGalleryRepository;
            _mapper = mapper;
        }

        public RoomDto GetRoom(int roomId)
        {
            if (!_roomRepository.IsExists(roomId)) return null;
            var room = _roomRepository.GetByIdDto(roomId);
            return room;
        }

        public IEnumerable<RoomDto> GetRooms()
        {
            return _roomRepository.GetAll();
        }
        public ResponseModel CreateRoom(RoomDto roomCreate)
        {
            if (roomCreate.CreatedBy == 0) roomCreate.CreatedBy = 1;
            if (roomCreate.UpdatedBy == 0) roomCreate.UpdatedBy = 1;
            roomCreate.CreatedAt = DateTime.Now;
            roomCreate.IsActive = false;
            roomCreate.IsDeleted = false;
            var rooms = _roomRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == roomCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (rooms != null)
            {
                return new ResponseModel(422, "Room already exists");
            }


            var roomMap = _mapper.Map<Room>(roomCreate);
            roomMap.CreatedAt = DateTime.Now;


            if (!_roomRepository.Create(roomMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            foreach (var img in roomCreate.Images)
            {
                var roomGallery = new RoomGallery()
                {
                    RoomId = roomMap.Id,
                    Image = img,
                    CreatedBy = roomCreate.CreatedBy,
                    UpdatedBy = roomCreate.UpdatedBy,
                    IsDeleted = false
                };
                var checkImgExist = _roomGalleryRepository.GetAll().Any(img =>
                                                                    img.RoomId == roomGallery.RoomId &&
                                                                    img.Image == roomGallery.Image);
                if (checkImgExist) continue;
                if (!_roomGalleryRepository.Create(roomGallery))
                {
                    return new ResponseModel(500, "Something went wrong while saving images");
                }
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateRoom(int roomId, RoomDto updatedRoom)
        {
            if (updatedRoom.CreatedBy == 0) updatedRoom.CreatedBy = 1;
            if (updatedRoom.UpdatedBy == 0) updatedRoom.UpdatedBy = 1;
            updatedRoom.UpdatedAt = DateTime.Now;

            if (!_roomRepository.IsExists(roomId)) return new ResponseModel(404, "Not found");
            var roomMap = _mapper.Map<Room>(updatedRoom);
            if (!_roomRepository.Update(roomMap))
            {
                return new ResponseModel(500, "Something went wrong updating room");
            }

            //Images
            foreach (var img in updatedRoom.Images)
            {
                var roomGallery = new RoomGallery()
                {
                    RoomId = roomMap.Id,
                    Image = img,
                    CreatedBy = updatedRoom.CreatedBy,
                    UpdatedBy = updatedRoom.UpdatedBy,
                    IsDeleted = false
                };
                var checkImgExist = _roomGalleryRepository.GetAll().Any(img =>
                                                                    img.RoomId == roomGallery.RoomId &&
                                                                    img.Image == roomGallery.Image);
                if (checkImgExist) continue;
                if (!_roomGalleryRepository.Create(roomGallery))
                {
                    return new ResponseModel(500, "Something went wrong while saving images");
                }
            }
            if (!_roomGalleryRepository.UpdateStatus(roomId, updatedRoom.Images))
            {
                return new ResponseModel(500, "Something went wrong updating status of images room");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteRoom(int roomId)
        {
            if (!_roomRepository.IsExists(roomId)) return new ResponseModel(404, "Not found");
            var roomToDelete = _roomRepository.GetById(roomId);
            if (!_roomRepository.Delete(roomToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting room");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<RoomDto> SearchRooms(string field, string keyWords)
        {
            var res = _roomRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateRoom(int roomId, bool isDelete)
        {
            if (!_roomRepository.SetDelete(roomId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete room");
            }
            return new ResponseModel(204, "");
        }
    }
}
