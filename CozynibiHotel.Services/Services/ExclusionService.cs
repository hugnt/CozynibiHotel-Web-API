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
    public class ExclusionService : IExclusionService
    {
        private readonly IExclusionRepository _exclusionRepository;
        private readonly IMapper _mapper;

        public ExclusionService(IExclusionRepository exclusionRepository, IMapper mapper)
        {
            _exclusionRepository = exclusionRepository;
            _mapper = mapper;
        }

        public ExclusionDto GetExclusion(int exclusionId)
        {
            if (!_exclusionRepository.IsExists(exclusionId)) return null;
            var exclusion = _mapper.Map<ExclusionDto>(_exclusionRepository.GetById(exclusionId));
            return exclusion;
        }

        public IEnumerable<ExclusionDto> GetExclusions()
        {
            var exclusions = _mapper.Map<List<ExclusionDto>>(_exclusionRepository.GetAll());
            return exclusions;
        }
        public ResponseModel CreateExclusion(ExclusionDto exclusionCreate)
        {
            var exclusions = _exclusionRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == exclusionCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (exclusions != null)
            {
                return new ResponseModel(422, "Exclusion already exists");
            }

            var exclusionMap = _mapper.Map<Exclusion>(exclusionCreate);

            if (!_exclusionRepository.Create(exclusionMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateExclusion(int exclusionId, ExclusionDto updatedExclusion)
        {
            if (!_exclusionRepository.IsExists(exclusionId)) return new ResponseModel(404,"Not found");
            var exclusionMap = _mapper.Map<Exclusion>(updatedExclusion);
            if (!_exclusionRepository.Update(exclusionMap))
            {
                return new ResponseModel(500, "Something went wrong updating exclusion");
            }
            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteExclusion(int exclusionId)
        {
            if (!_exclusionRepository.IsExists(exclusionId)) return new ResponseModel(404, "Not found");
            var exclusionToDelete = _exclusionRepository.GetById(exclusionId);
            if (!_exclusionRepository.Delete(exclusionToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting exclusion");
            }
            return new ResponseModel(204, "");
        }
    }
}
