using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Infrastructure.Data;
using HUG.CRUD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Infrastructure.Repository
{
    public class InformationRepository : GenericRepository<Information>, IInformationRepository
    {
        public InformationRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

    }
}
