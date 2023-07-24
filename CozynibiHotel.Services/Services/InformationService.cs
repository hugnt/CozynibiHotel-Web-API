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
    public class InformationService : IInformationService
    {
        private readonly IInformationRepository _informationRepository;
        private readonly IMapper _mapper;

        public InformationService(IInformationRepository informationRepository, IMapper mapper)
        {
            _informationRepository = informationRepository;
            _mapper = mapper;
        }

        public InformationDto GetInformation()
        {
            if (!_informationRepository.IsExists(1)) return null;
            var information = _mapper.Map<InformationDto>(_informationRepository.GetById(1));
            return information;
        }

        public ResponseModel UpdateInformation(InformationDto updatedInformation)
        {
            if (!_informationRepository.IsExists(1)) return new ResponseModel(404,"Not found");
            var informationMap = _mapper.Map<Information>(updatedInformation);
            if (!_informationRepository.Update(informationMap))
            {
                return new ResponseModel(500, "Something went wrong updating information");
            }
            return new ResponseModel(204, "");

        }



    }
}
