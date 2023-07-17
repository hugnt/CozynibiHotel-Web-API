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
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository,
                            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public ServiceDto GetService(int serviceId)
        {
            if (!_serviceRepository.IsExists(serviceId)) return null;
            var serviceMap = _mapper.Map<ServiceDto>(_serviceRepository.GetById(serviceId));
            return serviceMap;
        }

        public IEnumerable<ServiceDto> GetServices()
        {
            var servicesMap = _mapper.Map<List<ServiceDto>>(_serviceRepository.GetAll());
            return servicesMap;
        }
        public ResponseModel CreateService(ServiceDto serviceCreate)
        {
            if (serviceCreate.CreatedBy == 0) serviceCreate.CreatedBy = 1;
            if (serviceCreate.UpdatedBy == 0) serviceCreate.UpdatedBy = 1;
            serviceCreate.CreatedAt = DateTime.Now;
            serviceCreate.IsActive = false;
            serviceCreate.IsDeleted = false;
            var services = _serviceRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == serviceCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (services != null)
            {
                return new ResponseModel(422, "Service already exists");
            }


            var serviceMap = _mapper.Map<Service>(serviceCreate);
            serviceMap.CreatedAt = DateTime.Now;


            if (!_serviceRepository.Create(serviceMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateService(int serviceId, ServiceDto updatedService)
        {
            if (updatedService.CreatedBy == 0) updatedService.CreatedBy = 1;
            if (updatedService.UpdatedBy == 0) updatedService.UpdatedBy = 1;
            updatedService.UpdatedAt = DateTime.Now;

            if (!_serviceRepository.IsExists(serviceId)) return new ResponseModel(404, "Not found");
            var serviceMap = _mapper.Map<Service>(updatedService);
            if (!_serviceRepository.Update(serviceMap))
            {
                return new ResponseModel(500, "Something went wrong updating service");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteService(int serviceId)
        {
            if (!_serviceRepository.IsExists(serviceId)) return new ResponseModel(404, "Not found");
            var serviceToDelete = _serviceRepository.GetById(serviceId);
            if (!_serviceRepository.Delete(serviceToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting service");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<ServiceDto> SearchServices(string field, string keyWords)
        {
            var res = _serviceRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateService(int serviceId, bool isDelete)
        {
            if (!_serviceRepository.SetDelete(serviceId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete service");
            }
            return new ResponseModel(204, "");
        }
    }
}
