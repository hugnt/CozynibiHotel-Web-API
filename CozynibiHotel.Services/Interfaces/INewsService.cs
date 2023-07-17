using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface INewsService
    {
        IEnumerable<NewsDto> GetNews();
        IEnumerable<NewsDto> SearchNews(string field, string keyWords);
        NewsDto GetNews(int newsId);
        ResponseModel CreateNews(NewsDto newsCreate);
        ResponseModel UpdateNews(int newsId, NewsDto updatedNews);
        ResponseModel UpdateNews(int newsId, bool isDelete);
        ResponseModel DeleteNews(int newsCategoryId);

    }
}
