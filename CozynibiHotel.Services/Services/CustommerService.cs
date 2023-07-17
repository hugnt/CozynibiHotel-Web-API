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
    public class CustommerService : ICustommerService
    {
        private readonly ICustommerRepository _custommerRepository;
        private readonly IMapper _mapper;

        public CustommerService(ICustommerRepository custommerRepository,
                            IMapper mapper)
        {
            _custommerRepository = custommerRepository;
            _mapper = mapper;
        }

        public CustommerDto GetCustommer(int custommerId)
        {
            if (!_custommerRepository.IsExists(custommerId)) return null;
            var custommerMap = _mapper.Map<CustommerDto>(_custommerRepository.GetById(custommerId));
            return custommerMap;
        }

        public IEnumerable<CustommerDto> GetCustommers()
        {
            var custommersMap = _mapper.Map<List<CustommerDto>>(_custommerRepository.GetAll());
            return custommersMap;
        }
        public ResponseModel CreateCustommer(CustommerDto custommerCreate)
        {
            if (custommerCreate.CreatedBy == 0) custommerCreate.CreatedBy = 1;
            if (custommerCreate.UpdatedBy == 0) custommerCreate.UpdatedBy = 1;
            custommerCreate.CreatedAt = DateTime.Now;
            custommerCreate.IsActive = false;
            custommerCreate.IsDeleted = false;
            var custommers = _custommerRepository.GetAll()
                            .Where(l => l.FullName.Trim().ToLower() == custommerCreate.FullName.Trim().ToLower())
                            .FirstOrDefault();
            if (custommers != null)
            {
                return new ResponseModel(422, "Custommer already exists");
            }


            var custommerMap = _mapper.Map<Custommer>(custommerCreate);
            custommerMap.CreatedAt = DateTime.Now;


            if (!_custommerRepository.Create(custommerMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateCustommer(int custommerId, CustommerDto updatedCustommer)
        {
            if (updatedCustommer.CreatedBy == 0) updatedCustommer.CreatedBy = 1;
            if (updatedCustommer.UpdatedBy == 0) updatedCustommer.UpdatedBy = 1;
            updatedCustommer.UpdatedAt = DateTime.Now;

            if (!_custommerRepository.IsExists(custommerId)) return new ResponseModel(404, "Not found");
            var custommerMap = _mapper.Map<Custommer>(updatedCustommer);
            if (!_custommerRepository.Update(custommerMap))
            {
                return new ResponseModel(500, "Something went wrong updating custommer");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteCustommer(int custommerId)
        {
            if (!_custommerRepository.IsExists(custommerId)) return new ResponseModel(404, "Not found");
            var custommerToDelete = _custommerRepository.GetById(custommerId);
            if (!_custommerRepository.Delete(custommerToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting custommer");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<CustommerDto> SearchCustommers(string field, string keyWords)
        {
            var res = _custommerRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateCustommer(int custommerId, bool isDelete)
        {
            if (!_custommerRepository.SetDelete(custommerId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete custommer");
            }
            return new ResponseModel(204, "");
        }
    }
}
