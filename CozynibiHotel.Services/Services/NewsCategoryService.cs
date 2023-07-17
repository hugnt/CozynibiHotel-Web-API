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
    public class NewsCategoryService : INewsCategoryService
    {
        private readonly INewsCategoryRepository _newsCategoryRepository;
        private readonly IMapper _mapper;

        public NewsCategoryService(INewsCategoryRepository newsCategoryRepository, 
                                   IMapper mapper)
        {
            _newsCategoryRepository = newsCategoryRepository;
            _mapper = mapper;
        }

        public NewsCategoryDto GetNewsCategory(int newsCategoryId)
        {
            if (!_newsCategoryRepository.IsExists(newsCategoryId)) return null;
            var newsCategory = _newsCategoryRepository.GetById(newsCategoryId);
            var newsCategoryMap = _mapper.Map<NewsCategoryDto>(newsCategory);
            return newsCategoryMap;
        }
        public IEnumerable<NewsCategoryDto> GetNewsCategories()
        {
            var newsCategoryMap = _mapper.Map<List<NewsCategoryDto>>(_newsCategoryRepository.GetAll());
            return newsCategoryMap;
        }
        public ResponseModel CreateNewsCategory(NewsCategoryDto newsCategoryCreate)
        {
            if (newsCategoryCreate.CreatedBy == 0) newsCategoryCreate.CreatedBy = 1;
            if (newsCategoryCreate.UpdatedBy == 0) newsCategoryCreate.UpdatedBy = 1;
            newsCategoryCreate.CreatedAt = DateTime.Now;
            newsCategoryCreate.IsActive = false;
            newsCategoryCreate.IsDeleted = false;
            var newsCategories = _newsCategoryRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == newsCategoryCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (newsCategories != null)
            {
                return new ResponseModel(422, "NewsCategory already exists");
            }

            
            var newsCategoryMap = _mapper.Map<NewsCategory>(newsCategoryCreate);
            newsCategoryMap.CreatedAt = DateTime.Now;


            if (!_newsCategoryRepository.Create(newsCategoryMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateNewsCategory(int newsCategoryId, NewsCategoryDto updatedNewsCategory)
        {
            if (updatedNewsCategory.CreatedBy == 0) updatedNewsCategory.CreatedBy = 1;
            if (updatedNewsCategory.UpdatedBy == 0) updatedNewsCategory.UpdatedBy = 1;
            updatedNewsCategory.UpdatedAt = DateTime.Now;

            if (!_newsCategoryRepository.IsExists(newsCategoryId)) return new ResponseModel(404,"Not found");
            var newsCategoryMap = _mapper.Map<NewsCategory>(updatedNewsCategory);
            if (!_newsCategoryRepository.Update(newsCategoryMap))
            {
                return new ResponseModel(500, "Something went wrong updating newsCategory");
            }
            return new ResponseModel(204, "");

        }


        public ResponseModel UpdateNewsCategory(int newsCategoryId, bool isDelete)
        {
            if(!_newsCategoryRepository.SetDelete(newsCategoryId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete newsCategory");
            }
            return new ResponseModel(204, "");
        }

        public ResponseModel DeleteNewsCategory(int newsCategoryId)
        {
            if (!_newsCategoryRepository.IsExists(newsCategoryId)) return new ResponseModel(404, "Not found");
            var newsCategoryToDelete = _newsCategoryRepository.GetById(newsCategoryId);
            if (!_newsCategoryRepository.Delete(newsCategoryToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting newsCategory");
            }

            return new ResponseModel(204, "");
        }

        public IEnumerable<NewsCategoryDto> SearchNewsCategories(string field, string keyWords)
        {
            var res = _mapper.Map<List<NewsCategoryDto>>(_newsCategoryRepository.Search(field, keyWords));
            return res;
        }
    }
}
