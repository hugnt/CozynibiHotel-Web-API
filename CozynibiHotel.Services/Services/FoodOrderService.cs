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
    public class FoodOrderService : IFoodOrderService
    {
        private readonly IFoodOrderRepository _foodOrderRepository;
        private readonly IMapper _mapper;
        private readonly IFoodOrderDetailsRepository _foodOrderDetailsRepository;
        private readonly ICustommerRepository _custommerRepository;

        public FoodOrderService(IFoodOrderRepository foodOrderRepository, 
                                IMapper mapper,
                                IFoodOrderDetailsRepository foodOrderDetailsRepository,
                                ICustommerRepository custommerRepository)
        {
            _foodOrderRepository = foodOrderRepository;
            _mapper = mapper;
            _foodOrderDetailsRepository = foodOrderDetailsRepository;
            _custommerRepository = custommerRepository;
        }

        public FoodOrderDto GetFoodOrder(int foodOrderId)
        {
            if (!_foodOrderRepository.IsExists(foodOrderId)) return null;
            var foodOrder = _foodOrderRepository.GetByIdDto(foodOrderId);
            return foodOrder;
        }
        public IEnumerable<FoodOrderDto> GetFoodOrders()
        {
            return _foodOrderRepository.GetAll();
        }
        public ResponseModel CreateFoodOrder(FoodOrderDto foodOrderCreate)
        {
            if (foodOrderCreate.CreatedBy == 0) foodOrderCreate.CreatedBy = 1;
            if (foodOrderCreate.UpdatedBy == 0) foodOrderCreate.UpdatedBy = 1;
            foodOrderCreate.CreatedAt = DateTime.Now;
            foodOrderCreate.IsActive = false;
            foodOrderCreate.IsDeleted = false;
            var foodOrders = _foodOrderRepository.GetAll()
                            .Where(l => l.FullName.Trim().ToLower() == foodOrderCreate.FullName.Trim().ToLower()
                                   && l.Place == foodOrderCreate.Place
                                   && l.CheckInCode == foodOrderCreate.CheckInCode
                                   && l.EatingAt == l.EatingAt)
                            .FirstOrDefault();
            if (foodOrders != null)
            {
                return new ResponseModel(422, "FoodOrder already exists");
            }

            
            var foodOrderMap = _mapper.Map<FoodOrder>(foodOrderCreate);
            foodOrderMap.CreatedAt = DateTime.Now;


            if (!_foodOrderRepository.Create(foodOrderMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            foreach (var fd in foodOrderCreate.FoodList)
            {
                //Create relation ship
                var foodOrderDetails= new FoodOrderDetails()
                {
                    FoodOrderId = foodOrderMap.Id,
                    FoodId = fd.FoodId,
                    Number = fd.Number,
                    CreatedBy = foodOrderCreate.CreatedBy,
                    UpdatedBy = foodOrderCreate.UpdatedBy,
                    IsDeleted = false

                };
                var checkFoodOrderDetailsExist = _foodOrderDetailsRepository.GetAll().Any(fd =>
                                                                    fd.FoodOrderId == foodOrderDetails.FoodOrderId &&
                                                                    fd.FoodId == foodOrderDetails.FoodId&&
                                                                    fd.Number == foodOrderDetails.Number);
                if (checkFoodOrderDetailsExist) continue;
                if (!_foodOrderDetailsRepository.Create(foodOrderDetails))
                {
                    return new ResponseModel(500, "Something went wrong while saving food order details");
                }
            }

            return new ResponseModel(201, "Successfully created");

        }

        public ResponseModel UpdateFoodOrder(int foodOrderId, FoodOrderDto updatedFoodOrder)
        {
            if (updatedFoodOrder.CreatedBy == 0) updatedFoodOrder.CreatedBy = 1;
            if (updatedFoodOrder.UpdatedBy == 0) updatedFoodOrder.UpdatedBy = 1;
            updatedFoodOrder.UpdatedAt = DateTime.Now;

            if (!_foodOrderRepository.IsExists(foodOrderId)) return new ResponseModel(404,"Not found");
            var foodOrderMap = _mapper.Map<FoodOrder>(updatedFoodOrder);
            if (!_foodOrderRepository.Update(foodOrderMap))
            {
                return new ResponseModel(500, "Something went wrong updating foodOrder");
            }

            //Othes
            //Reset 
            if (!_foodOrderDetailsRepository.SetDeletedAll(foodOrderId))
            {
                return new ResponseModel(500, "Something went wrong reseting status of foodOrderDetails");
            }
            foreach (var fd in updatedFoodOrder.FoodList)
            {
                //Create relation ship
                var foodOrderDetails= new FoodOrderDetails()
                {
                    FoodOrderId = foodOrderMap.Id,
                    FoodId = fd.FoodId,
                    Number = fd.Number,
                    CreatedBy = updatedFoodOrder.CreatedBy,
                    UpdatedBy = updatedFoodOrder.UpdatedBy,
                    IsDeleted = false

                };
                var checkFoodOrderDetailsExist = _foodOrderDetailsRepository.GetAll().Any(fd =>
                                                                   fd.FoodOrderId == foodOrderDetails.FoodOrderId &&
                                                                   fd.FoodId == foodOrderDetails.FoodId &&
                                                                   fd.Number == foodOrderDetails.Number);
                if (checkFoodOrderDetailsExist) 
                {
                    if (!_foodOrderDetailsRepository.Update(foodOrderDetails))
                    {
                        return new ResponseModel(500, "Something went wrong while updating foodOrderDetails");
                    }
                    continue;
                }
                if (!_foodOrderDetailsRepository.Create(foodOrderDetails))
                {
                    return new ResponseModel(500, "Something went wrong while saving food order details");
                }
            }

            return new ResponseModel(204, "");

        }


        public ResponseModel UpdateFoodOrder(int foodOrderId, bool isDelete)
        {
            if(!_foodOrderRepository.SetDelete(foodOrderId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete foodOrder");
            }
            return new ResponseModel(204, "");
        }
        public ResponseModel UpdateFoodOrderStatus(int foodOrderId, bool status)
        {
            if (!_foodOrderRepository.SetStatus(foodOrderId, status))
            {
                return new ResponseModel(500, "Something went wrong when updaing status FoodOrder");
            }
            
            return new ResponseModel(204, "");
        }

        public ResponseModel DeleteFoodOrder(int foodOrderId)
        {
            if (!_foodOrderRepository.IsExists(foodOrderId)) return new ResponseModel(404, "Not found");
            var foodOrderToDelete = _foodOrderRepository.GetById(foodOrderId);
            if (!_foodOrderRepository.Delete(foodOrderToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting foodOrder");
            }

            return new ResponseModel(204, "");
        }

        public IEnumerable<FoodOrderDto> SearchFoodOrders(string field, string keyWords)
        {
            var res = _foodOrderRepository.Search(field, keyWords);
            return res;
        }

        public bool IsValidCheckInCode(int checkInCode)
        {
            var custommerByCheckinCode = _custommerRepository.GetAll().FirstOrDefault(x => x.IsActive == true&&x.CheckInCode == checkInCode);
            if (custommerByCheckinCode == null) return false;
            return true;
        }
    }
}
