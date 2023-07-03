using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IEquipmentService
    {
        IEnumerable<EquipmentDto> GetEquipments();
        EquipmentDto GetEquipment(int equipmentId);
        ResponseModel CreateEquipment(EquipmentDto equipmentCreate);
        ResponseModel UpdateEquipment(int equipmentId, EquipmentDto updatedEquipment);
        ResponseModel DeleteEquipment(int equipmentCategoryId);

    }
}
