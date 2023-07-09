using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IFoodService
    {
        IEnumerable<FoodDto> GetFoods();
        IEnumerable<FoodDto> SearchFoods(string field, string keyWords);
        FoodDto GetFood(int foodId);
        ResponseModel CreateFood(FoodDto foodCreate);
        ResponseModel UpdateFood(int foodId, FoodDto updatedFood);
        ResponseModel UpdateFood(int foodId, bool isDelete);
        ResponseModel DeleteFood(int foodCategoryId);

    }
}
