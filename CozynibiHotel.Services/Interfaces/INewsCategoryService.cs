using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface INewsCategoryService
    {
        IEnumerable<NewsCategoryDto> GetNewsCategories();
        IEnumerable<NewsCategoryDto> SearchNewsCategories(string field, string keyWords);
        NewsCategoryDto GetNewsCategory(int newsCategoryId);
        ResponseModel CreateNewsCategory(NewsCategoryDto newsCategoryCreate);
        ResponseModel UpdateNewsCategory(int newsCategoryId, NewsCategoryDto updatedNewsCategory);
        ResponseModel UpdateNewsCategory(int newsCategoryId, bool isDelete);
        ResponseModel DeleteNewsCategory(int newsCategoryId);

    }
}
