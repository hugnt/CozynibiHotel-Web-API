using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IExclusionService
    {
        IEnumerable<ExclusionDto> GetExclusions();
        ExclusionDto GetExclusion(int exclusionId);
        ResponseModel CreateExclusion(ExclusionDto exclusionCreate);
        ResponseModel UpdateExclusion(int exclusionId, ExclusionDto updatedExclusion);
        ResponseModel DeleteExclusion(int exclusionCategoryId);

    }
}
