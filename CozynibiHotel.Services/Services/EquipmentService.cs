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
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IMapper _mapper;

        public EquipmentService(IEquipmentRepository equipmentRepository, IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _mapper = mapper;
        }

        public EquipmentDto GetEquipment(int equipmentId)
        {
            if (!_equipmentRepository.IsExists(equipmentId)) return null;
            var equipment = _mapper.Map<EquipmentDto>(_equipmentRepository.GetById(equipmentId));
            return equipment;
        }

        public IEnumerable<EquipmentDto> GetEquipments()
        {
            var equipments = _mapper.Map<List<EquipmentDto>>(_equipmentRepository.GetAll());
            return equipments;
        }
        public ResponseModel CreateEquipment(EquipmentDto equipmentCreate)
        {
            var equipments = _equipmentRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == equipmentCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (equipments != null)
            {
                return new ResponseModel(422, "Equipment already exists");
            }

            var equipmentMap = _mapper.Map<Equipment>(equipmentCreate);

            if (!_equipmentRepository.Create(equipmentMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateEquipment(int equipmentId, EquipmentDto updatedEquipment)
        {
            if (!_equipmentRepository.IsExists(equipmentId)) return new ResponseModel(404,"Not found");
            var equipmentMap = _mapper.Map<Equipment>(updatedEquipment);
            if (!_equipmentRepository.Update(equipmentMap))
            {
                return new ResponseModel(500, "Something went wrong updating equipment");
            }
            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteEquipment(int equipmentId)
        {
            if (!_equipmentRepository.IsExists(equipmentId)) return new ResponseModel(404, "Not found");
            var equipmentToDelete = _equipmentRepository.GetById(equipmentId);
            if (!_equipmentRepository.Delete(equipmentToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting equipment");
            }
            return new ResponseModel(204, "");
        }
    }
}
