using CozynibiHotel.Core.Models;
using HUG.CRUD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Interfaces
{
    public interface IPageBannerRepository : IGenericRepository<PageBanner>
    {
        bool UpdateStatus(int category_id, List<string> lstUpdateImage);
    }
}
