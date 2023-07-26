using AutoMapper;
using CozynibiHotel.Core.Dto;
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
    public class FoodOrderRepository : GenericRepository<FoodOrder>, IFoodOrderRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public FoodOrderRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public ICollection<FoodOrderDto> GetAll()
        {
            var foodOrders = new List<FoodOrderDto>();


            var foodOrderDetails = from fod in _dbContext.FoodOrderDetails
                                join f in _dbContext.Foods on fod.FoodId equals f.Id
                                where fod.IsDeleted == false
                                select new
                                  {
                                      FoodOrderId = fod.FoodOrderId,
                                      FoodId = fod.FoodId,
                                      Number = fod.Number,
                                  };

    
            var foodOrderGroupJoin = from fo in _dbContext.FoodOrders.ToList()
                                      join fod in foodOrderDetails on fo.Id equals fod.FoodOrderId into fd
                                      select new
                                      {
                                          FoodOrder = fo,
                                          FoodOrderDetails = fd,
                                      };

            foreach (var item in foodOrderGroupJoin)
            {
                var foodOrder = _mapper.Map<FoodOrderDto>(item.FoodOrder);
                foreach (var fd in item.FoodOrderDetails)
                {
                    foodOrder.FoodList.Add(new FoodOrderDetailsDto()
                    {
                        FoodId = fd.FoodId,
                        Number = fd.Number,
                    });
                }
                foodOrders.Add(foodOrder);
            }

            return foodOrders;
            

        }

        public FoodOrderDto GetByIdDto(int foodOrderId)
        {
            return GetAll().FirstOrDefault(e => e.Id == foodOrderId);
        }

        public bool SetDelete(int id, bool isDelete)
        {
            try
            {
                var selectedRecord = _dbContext.FoodOrders.Find(id);
                if (selectedRecord != null)
                {
                    selectedRecord.IsDeleted = isDelete;
                    selectedRecord.IsActive = false;
                    Update(selectedRecord);
                }

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        public bool SetStatus(int id, bool status)
        {
            try
            {
                var selectedRecord = _dbContext.FoodOrders.Find(id);
                if (selectedRecord != null)
                {
                    selectedRecord.IsActive = status;
                    Update(selectedRecord);
                }

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        public ICollection<FoodOrderDto> Search(string field, string keyWords)
        {
            if (keyWords == "" || keyWords == "*" || keyWords == null) return GetAll();
            field = field.ToLower();
            field = field.Substring(0, 1).ToUpper() + field.Substring(1);
            keyWords = keyWords.ToLower();
            if (field == "Isactive")
            {

                if (keyWords == "1" || keyWords.Contains("Active") || keyWords == "true")
                {
                    return GetAll().Where(e => e.IsActive == true).ToList();
                }
                else
                {
                    return GetAll().Where(e => e.IsActive == false).ToList();
                }

            }

            var res = GetAll()
            .Where(e => e.GetType().GetProperty(field)?
            .GetValue(e)?
            .ToString()?
            .ToLower()
            .Contains(keyWords) ?? false)
            .ToList();
            if (res.Count() > 0) return res;

            return null;
        }
    }
}
