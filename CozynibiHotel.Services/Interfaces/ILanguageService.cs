using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface ILanguageService
    {
        IEnumerable<LanguageDto> GetLanguages();
        LanguageDto GetLanguage(int languageId);
        ResponseModel CreateLanguage(LanguageDto languageCreate);
        ResponseModel UpdateLanguage(int languageId, LanguageDto updatedLanguage);
        ResponseModel DeleteLanguage(int languageId);

    }
}
