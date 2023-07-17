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
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository,
                            IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }

        public ArticleDto GetArticle(int articleId)
        {
            if (!_articleRepository.IsExists(articleId)) return null;
            var articleMap = _mapper.Map<ArticleDto>(_articleRepository.GetById(articleId));
            return articleMap;
        }

        public IEnumerable<ArticleDto> GetArticles()
        {
            var articlesMap = _mapper.Map<List<ArticleDto>>(_articleRepository.GetAll());
            return articlesMap;
        }
        public ResponseModel CreateArticle(ArticleDto articleCreate)
        {
            if (articleCreate.CreatedBy == 0) articleCreate.CreatedBy = 1;
            if (articleCreate.UpdatedBy == 0) articleCreate.UpdatedBy = 1;
            articleCreate.CreatedAt = DateTime.Now;
            articleCreate.IsActive = false;
            articleCreate.IsDeleted = false;
            var articles = _articleRepository.GetAll()
                            .Where(l => l.Title.Trim().ToLower() == articleCreate.Title.Trim().ToLower()
                                        && l.SubTitle.Trim().ToLower() == articleCreate.SubTitle.Trim().ToLower()
                                        && l.PageId == articleCreate.PageId)
                            .FirstOrDefault();
            if (articles != null)
            {
                return new ResponseModel(422, "Article already exists");
            }


            var articleMap = _mapper.Map<Article>(articleCreate);
            articleMap.CreatedAt = DateTime.Now;


            if (!_articleRepository.Create(articleMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateArticle(int articleId, ArticleDto updatedArticle)
        {
            if (updatedArticle.CreatedBy == 0) updatedArticle.CreatedBy = 1;
            if (updatedArticle.UpdatedBy == 0) updatedArticle.UpdatedBy = 1;
            updatedArticle.UpdatedAt = DateTime.Now;

            if (!_articleRepository.IsExists(articleId)) return new ResponseModel(404, "Not found");
            var articleMap = _mapper.Map<Article>(updatedArticle);
            if (!_articleRepository.Update(articleMap))
            {
                return new ResponseModel(500, "Something went wrong updating article");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteArticle(int articleId)
        {
            if (!_articleRepository.IsExists(articleId)) return new ResponseModel(404, "Not found");
            var articleToDelete = _articleRepository.GetById(articleId);
            if (!_articleRepository.Delete(articleToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting article");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<ArticleDto> SearchArticles(string field, string keyWords)
        {
            var res = _articleRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateArticle(int articleId, bool isDelete)
        {
            if (!_articleRepository.SetDelete(articleId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete article");
            }
            return new ResponseModel(204, "");
        }
    }
}
