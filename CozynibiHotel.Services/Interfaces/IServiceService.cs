using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IServiceService
    {
        IEnumerable<ServiceDto> GetServices();
        IEnumerable<ServiceDto> SearchServices(string field, string keyWords);
        ServiceDto GetService(int serviceId);
        ResponseModel CreateService(ServiceDto serviceCreate);
        ResponseModel UpdateService(int serviceId, ServiceDto updatedService);
        ResponseModel UpdateService(int serviceId, bool isDelete);
        ResponseModel DeleteService(int serviceCategoryId);

    }
}
