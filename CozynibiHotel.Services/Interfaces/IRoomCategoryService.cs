using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IRoomCategoryService
    {
        IEnumerable<RoomCategoryDto> GetRoomCategories();
        RoomCategoryDto GetRoomCategory(int roomCategoryId);
        ResponseModel CreateRoomCategory(RoomCategoryDto roomCategoryCreate);
        ResponseModel UpdateRoomCategory(int roomCategoryId, RoomCategoryDto updatedRoomCategory);
        ResponseModel DeleteRoomCategory(int roomCategoryId);

    }
}
