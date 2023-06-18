using AutoMapper;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Language, LanguageDto>();
            CreateMap<LanguageDto, Language>();
        }
        
    }
}
