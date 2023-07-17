using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IArticleService
    {
        IEnumerable<ArticleDto> GetArticles();
        IEnumerable<ArticleDto> SearchArticles(string field, string keyWords);
        ArticleDto GetArticle(int articleId);
        ResponseModel CreateArticle(ArticleDto articleCreate);
        ResponseModel UpdateArticle(int articleId, ArticleDto updatedArticle);
        ResponseModel UpdateArticle(int articleId, bool isDelete);
        ResponseModel DeleteArticle(int articleCategoryId);

    }
}
