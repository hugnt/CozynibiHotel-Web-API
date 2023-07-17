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
    public class TourScheduleRepository : GenericRepository<TourSchedule>, ITourScheduleRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public TourScheduleRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool SetDeletedAll(int tour_id)
        {
            try
            {
                var all = GetAll().Where(s => s.TourId == tour_id);
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

        public bool UpdateStatus(int scheduleId, bool status)
        {
            try
            {
                var selectedSchedule = GetById(scheduleId);
                if (selectedSchedule == null) return false;
                selectedSchedule.IsDeleted = false;
                Update(selectedSchedule);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
