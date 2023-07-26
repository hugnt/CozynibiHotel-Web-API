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
    public class FoodOrderDetailsRepository : GenericRepository<FoodOrderDetails>, IFoodOrderDetailsRepository
    {
        private readonly AppDbContext _dbContext;
        public FoodOrderDetailsRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public bool SetDeletedAll(int fooodOrderId)
        {
            try
            {
                var all = GetAll().Where(s => s.FoodOrderId == fooodOrderId);
                foreach (var item in all)
                {
                    item.IsDeleted = true;
                    Update(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
