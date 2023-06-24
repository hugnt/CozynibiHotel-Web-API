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
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public RoomDto GetRoom(int roomId)
        {
            if (!_roomRepository.IsExists(roomId)) return null;
            var room = _mapper.Map<RoomDto>(_roomRepository.GetById(roomId));
            return room;
        }

        public IEnumerable<RoomDto> GetRooms()
        {
            var rooms = _mapper.Map<List<RoomDto>>(_roomRepository.GetAll());
            return rooms;
        }
        public ResponseModel CreateRoom(RoomDto roomCreate)
        {
            var rooms = _roomRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == roomCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (rooms != null)
            {
                return new ResponseModel(422, "Room already exists");
            }

            var roomMap = _mapper.Map<Room>(roomCreate);

            if (!_roomRepository.Create(roomMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateRoom(int roomId, RoomDto updatedRoom)
        {
            if (!_roomRepository.IsExists(roomId)) return new ResponseModel(404,"Not found");
            var roomMap = _mapper.Map<Room>(updatedRoom);
            if (!_roomRepository.Update(roomMap))
            {
                return new ResponseModel(500, "Something went wrong updating room");
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
    }
}
