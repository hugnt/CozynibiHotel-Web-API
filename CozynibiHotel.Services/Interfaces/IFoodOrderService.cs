using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IFoodOrderService
    {
        IEnumerable<FoodOrderDto> GetFoodOrders();
        IEnumerable<FoodOrderDto> SearchFoodOrders(string field, string keyWords);
        FoodOrderDto GetFoodOrder(int foodOrderId);
        ResponseModel CreateFoodOrder(FoodOrderDto foodOrderCreate);
        ResponseModel UpdateFoodOrder(int foodOrderId, FoodOrderDto updatedFoodOrder);
        ResponseModel UpdateFoodOrder(int foodOrderId, bool isDelete);
        ResponseModel UpdateFoodOrderStatus(int foodOrderId, bool status);
        ResponseModel DeleteFoodOrder(int foodOrderId);
        bool IsValidCheckInCode(int checkInCode);

    }
}
