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
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public FoodService(IFoodRepository foodRepository,
                            IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        public FoodDto GetFood(int foodId)
        {
            if (!_foodRepository.IsExists(foodId)) return null;
            var foodMap = _mapper.Map<FoodDto>(_foodRepository.GetById(foodId));
            return foodMap;
        }

        public IEnumerable<FoodDto> GetFoods()
        {
            var foodsMap = _mapper.Map<List<FoodDto>>(_foodRepository.GetAll());
            return foodsMap;
        }
        public ResponseModel CreateFood(FoodDto foodCreate)
        {
            if (foodCreate.CreatedBy == 0) foodCreate.CreatedBy = 1;
            if (foodCreate.UpdatedBy == 0) foodCreate.UpdatedBy = 1;
            foodCreate.CreatedAt = DateTime.Now;
            foodCreate.IsActive = false;
            foodCreate.IsDeleted = false;
            var foods = _foodRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == foodCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (foods != null)
            {
                return new ResponseModel(422, "Food already exists");
            }


            var foodMap = _mapper.Map<Food>(foodCreate);
            foodMap.CreatedAt = DateTime.Now;


            if (!_foodRepository.Create(foodMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateFood(int foodId, FoodDto updatedFood)
        {
            if (updatedFood.CreatedBy == 0) updatedFood.CreatedBy = 1;
            if (updatedFood.UpdatedBy == 0) updatedFood.UpdatedBy = 1;
            updatedFood.UpdatedAt = DateTime.Now;

            if (!_foodRepository.IsExists(foodId)) return new ResponseModel(404, "Not found");
            var foodMap = _mapper.Map<Food>(updatedFood);
            if (!_foodRepository.Update(foodMap))
            {
                return new ResponseModel(500, "Something went wrong updating food");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteFood(int foodId)
        {
            if (!_foodRepository.IsExists(foodId)) return new ResponseModel(404, "Not found");
            var foodToDelete = _foodRepository.GetById(foodId);
            if (!_foodRepository.Delete(foodToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting food");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<FoodDto> SearchFoods(string field, string keyWords)
        {
            var res = _foodRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateFood(int foodId, bool isDelete)
        {
            if (!_foodRepository.SetDelete(foodId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete food");
            }
            return new ResponseModel(204, "");
        }
    }
}
