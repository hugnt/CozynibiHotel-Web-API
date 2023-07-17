using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IPageService
    {
        IEnumerable<PageDto> GetPages();
        IEnumerable<PageDto> SearchPages(string field, string keyWords);
        PageDto GetPage(int pageId);
        ResponseModel CreatePage(PageDto pageCreate);
        ResponseModel UpdatePage(int pageId, PageDto updatedPage);
        ResponseModel UpdatePage(int pageId, bool isDelete);
        ResponseModel DeletePage(int pageId);

    }
}
