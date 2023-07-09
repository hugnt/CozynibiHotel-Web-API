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
    public class FoodCategoryService : IFoodCategoryService
    {
        private readonly IFoodCategoryRepository _foodCategoryRepository;
        private readonly IMapper _mapper;

        public FoodCategoryService(IFoodCategoryRepository foodCategoryRepository, 
                                   IMapper mapper)
        {
            _foodCategoryRepository = foodCategoryRepository;
            _mapper = mapper;
        }

        public FoodCategoryDto GetFoodCategory(int foodCategoryId)
        {
            if (!_foodCategoryRepository.IsExists(foodCategoryId)) return null;
            var foodCategory = _foodCategoryRepository.GetById(foodCategoryId);
            var foodCategoryMap = _mapper.Map<FoodCategoryDto>(foodCategory);
            return foodCategoryMap;
        }
        public IEnumerable<FoodCategoryDto> GetFoodCategories()
        {
            var foodCategoryMap = _mapper.Map<List<FoodCategoryDto>>(_foodCategoryRepository.GetAll());
            return foodCategoryMap;
        }
        public ResponseModel CreateFoodCategory(FoodCategoryDto foodCategoryCreate)
        {
            if (foodCategoryCreate.CreatedBy == 0) foodCategoryCreate.CreatedBy = 1;
            if (foodCategoryCreate.UpdatedBy == 0) foodCategoryCreate.UpdatedBy = 1;
            foodCategoryCreate.CreatedAt = DateTime.Now;
            foodCategoryCreate.IsActive = false;
            foodCategoryCreate.IsDeleted = false;
            var foodCategories = _foodCategoryRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == foodCategoryCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (foodCategories != null)
            {
                return new ResponseModel(422, "FoodCategory already exists");
            }

            
            var foodCategoryMap = _mapper.Map<FoodCategory>(foodCategoryCreate);
            foodCategoryMap.CreatedAt = DateTime.Now;


            if (!_foodCategoryRepository.Create(foodCategoryMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateFoodCategory(int foodCategoryId, FoodCategoryDto updatedFoodCategory)
        {
            if (updatedFoodCategory.CreatedBy == 0) updatedFoodCategory.CreatedBy = 1;
            if (updatedFoodCategory.UpdatedBy == 0) updatedFoodCategory.UpdatedBy = 1;
            updatedFoodCategory.UpdatedAt = DateTime.Now;

            if (!_foodCategoryRepository.IsExists(foodCategoryId)) return new ResponseModel(404,"Not found");
            var foodCategoryMap = _mapper.Map<FoodCategory>(updatedFoodCategory);
            if (!_foodCategoryRepository.Update(foodCategoryMap))
            {
                return new ResponseModel(500, "Something went wrong updating foodCategory");
            }
            return new ResponseModel(204, "");

        }


        public ResponseModel UpdateFoodCategory(int foodCategoryId, bool isDelete)
        {
            if(!_foodCategoryRepository.SetDelete(foodCategoryId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete foodCategory");
            }
            return new ResponseModel(204, "");
        }

        public ResponseModel DeleteFoodCategory(int foodCategoryId)
        {
            if (!_foodCategoryRepository.IsExists(foodCategoryId)) return new ResponseModel(404, "Not found");
            var foodCategoryToDelete = _foodCategoryRepository.GetById(foodCategoryId);
            if (!_foodCategoryRepository.Delete(foodCategoryToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting foodCategory");
            }

            return new ResponseModel(204, "");
        }

        public IEnumerable<FoodCategoryDto> SearchFoodCategories(string field, string keyWords)
        {
            var res = _mapper.Map<List<FoodCategoryDto>>(_foodCategoryRepository.Search(field, keyWords));
            return res;
        }
    }
}
