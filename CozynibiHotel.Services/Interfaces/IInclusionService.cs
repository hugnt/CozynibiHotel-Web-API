using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IInclusionService
    {
        IEnumerable<InclusionDto> GetInclusions();
        InclusionDto GetInclusion(int inclusionId);
        ResponseModel CreateInclusion(InclusionDto inclusionCreate);
        ResponseModel UpdateInclusion(int inclusionId, InclusionDto updatedInclusion);
        ResponseModel DeleteInclusion(int inclusionCategoryId);

    }
}
