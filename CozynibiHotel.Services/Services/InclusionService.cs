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
    public class InclusionService : IInclusionService
    {
        private readonly IInclusionRepository _inclusionRepository;
        private readonly IMapper _mapper;

        public InclusionService(IInclusionRepository inclusionRepository, IMapper mapper)
        {
            _inclusionRepository = inclusionRepository;
            _mapper = mapper;
        }

        public InclusionDto GetInclusion(int inclusionId)
        {
            if (!_inclusionRepository.IsExists(inclusionId)) return null;
            var inclusion = _mapper.Map<InclusionDto>(_inclusionRepository.GetById(inclusionId));
            return inclusion;
        }

        public IEnumerable<InclusionDto> GetInclusions()
        {
            var inclusions = _mapper.Map<List<InclusionDto>>(_inclusionRepository.GetAll());
            return inclusions;
        }
        public ResponseModel CreateInclusion(InclusionDto inclusionCreate)
        {
            var inclusions = _inclusionRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == inclusionCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (inclusions != null)
            {
                return new ResponseModel(422, "Inclusion already exists");
            }

            var inclusionMap = _mapper.Map<Inclusion>(inclusionCreate);

            if (!_inclusionRepository.Create(inclusionMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateInclusion(int inclusionId, InclusionDto updatedInclusion)
        {
            if (!_inclusionRepository.IsExists(inclusionId)) return new ResponseModel(404,"Not found");
            var inclusionMap = _mapper.Map<Inclusion>(updatedInclusion);
            if (!_inclusionRepository.Update(inclusionMap))
            {
                return new ResponseModel(500, "Something went wrong updating inclusion");
            }
            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteInclusion(int inclusionId)
        {
            if (!_inclusionRepository.IsExists(inclusionId)) return new ResponseModel(404, "Not found");
            var inclusionToDelete = _inclusionRepository.GetById(inclusionId);
            if (!_inclusionRepository.Delete(inclusionToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting inclusion");
            }
            return new ResponseModel(204, "");
        }
    }
}
