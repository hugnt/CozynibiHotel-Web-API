using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IRoomService
    {
        IEnumerable<RoomDto> GetRooms();
        RoomDto GetRoom(int roomId);
        ResponseModel CreateRoom(RoomDto roomCreate);
        ResponseModel UpdateRoom(int roomId, RoomDto updatedRoom);
        ResponseModel DeleteRoom(int roomCategoryId);

    }
}
