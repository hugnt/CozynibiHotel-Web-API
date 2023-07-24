using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IInformationService
    {
        InformationDto GetInformation();
        ResponseModel UpdateInformation(InformationDto updatedInformation);

    }
}
