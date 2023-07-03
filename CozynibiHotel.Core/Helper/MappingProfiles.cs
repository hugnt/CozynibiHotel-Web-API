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
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();

            CreateMap<Language, LanguageDto>();
            CreateMap<LanguageDto, Language>();

            CreateMap<RoomCategory, RoomCategoryDto>();
            CreateMap<RoomCategoryDto, RoomCategory>();

            CreateMap<Room, RoomDto>();
            CreateMap<RoomDto, Room>();

            CreateMap<RefeshToken, RefeshTokenDto>();
            CreateMap<RefeshTokenDto, RefeshToken>();

            CreateMap<Equipment, EquipmentDto>();
            CreateMap<EquipmentDto, Equipment>();
        }
        
    }
}
