using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IFoodCategoryService
    {
        IEnumerable<FoodCategoryDto> GetFoodCategories();
        IEnumerable<FoodCategoryDto> SearchFoodCategories(string field, string keyWords);
        FoodCategoryDto GetFoodCategory(int foodCategoryId);
        ResponseModel CreateFoodCategory(FoodCategoryDto foodCategoryCreate);
        ResponseModel UpdateFoodCategory(int foodCategoryId, FoodCategoryDto updatedFoodCategory);
        ResponseModel UpdateFoodCategory(int foodCategoryId, bool isDelete);
        ResponseModel DeleteFoodCategory(int foodCategoryId);

    }
}
