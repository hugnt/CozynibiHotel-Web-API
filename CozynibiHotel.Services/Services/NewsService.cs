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
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public NewsService(INewsRepository newsRepository,
                            IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        public NewsDto GetNews(int newsId)
        {
            if (!_newsRepository.IsExists(newsId)) return null;
            var newsMap = _mapper.Map<NewsDto>(_newsRepository.GetById(newsId));
            return newsMap;
        }

        public IEnumerable<NewsDto> GetNews()
        {
            var newssMap = _mapper.Map<List<NewsDto>>(_newsRepository.GetAll());
            return newssMap;
        }
        public ResponseModel CreateNews(NewsDto newsCreate)
        {
            if (newsCreate.CreatedBy == 0) newsCreate.CreatedBy = 1;
            if (newsCreate.UpdatedBy == 0) newsCreate.UpdatedBy = 1;
            newsCreate.CreatedAt = DateTime.Now;
            newsCreate.IsActive = false;
            newsCreate.IsDeleted = false;
            var newss = _newsRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == newsCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (newss != null)
            {
                return new ResponseModel(422, "News already exists");
            }


            var newsMap = _mapper.Map<News>(newsCreate);
            newsMap.CreatedAt = DateTime.Now;


            if (!_newsRepository.Create(newsMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateNews(int newsId, NewsDto updatedNews)
        {
            if (updatedNews.CreatedBy == 0) updatedNews.CreatedBy = 1;
            if (updatedNews.UpdatedBy == 0) updatedNews.UpdatedBy = 1;
            updatedNews.UpdatedAt = DateTime.Now;

            if (!_newsRepository.IsExists(newsId)) return new ResponseModel(404, "Not found");
            var newsMap = _mapper.Map<News>(updatedNews);
            if (!_newsRepository.Update(newsMap))
            {
                return new ResponseModel(500, "Something went wrong updating news");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteNews(int newsId)
        {
            if (!_newsRepository.IsExists(newsId)) return new ResponseModel(404, "Not found");
            var newsToDelete = _newsRepository.GetById(newsId);
            if (!_newsRepository.Delete(newsToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting news");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<NewsDto> SearchNews(string field, string keyWords)
        {
            var res = _newsRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateNews(int newsId, bool isDelete)
        {
            if (!_newsRepository.SetDelete(newsId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete news");
            }
            return new ResponseModel(204, "");
        }
    }
}
