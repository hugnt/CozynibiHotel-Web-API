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
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IMapper _mapper;

        public LanguageService(ILanguageRepository languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public LanguageDto GetLanguage(int languageId)
        {
            if (!_languageRepository.IsExists(languageId)) return null;
            var language = _mapper.Map<LanguageDto>(_languageRepository.GetById(languageId));
            return language;
        }

        public IEnumerable<LanguageDto> GetLanguages()
        {
            var languages = _mapper.Map<List<LanguageDto>>(_languageRepository.GetAll());
            return languages;
        }
        public ResponseModel CreateLanguage(LanguageDto languageCreate)
        {
            var languages = _languageRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == languageCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (languages != null)
            {
                return new ResponseModel(422, "Language already exists");
            }

            var languageMap = _mapper.Map<Language>(languageCreate);

            if (!_languageRepository.Create(languageMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateLanguage(int languageId, LanguageDto updatedLanguage)
        {
            if (!_languageRepository.IsExists(languageId)) return new ResponseModel(404,"Not found");
            var languageMap = _mapper.Map<Language>(updatedLanguage);
            if (!_languageRepository.Update(languageMap))
            {
                return new ResponseModel(500, "Something went wrong updating language");
            }
            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteLanguage(int languageId)
        {
            if (!_languageRepository.IsExists(languageId)) return new ResponseModel(404, "Not found");
            var languageToDelete = _languageRepository.GetById(languageId);
            if (!_languageRepository.Delete(languageToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting language");
            }
            return new ResponseModel(204, "");
        }

        


    }
}
