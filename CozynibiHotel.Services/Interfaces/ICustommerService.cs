using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface ICustommerService
    {
        IEnumerable<CustommerDto> GetCustommers();
        IEnumerable<CustommerDto> SearchCustommers(string field, string keyWords);
        CustommerDto GetCustommer(int custommerId);
        ResponseModel CreateCustommer(CustommerDto custommerCreate);
        ResponseModel UpdateCustommer(int custommerId, CustommerDto updatedCustommer);
        ResponseModel UpdateCustommer(int custommerId, bool isDelete);
        ResponseModel DeleteCustommer(int custommerCategoryId);

    }
}
