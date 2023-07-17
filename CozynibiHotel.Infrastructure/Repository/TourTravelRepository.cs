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
    public class TourTravelRepository : GenericRepository<TourTravel>, ITourTravelRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public TourTravelRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public ICollection<TourTravelDto> GetAll()
        {
            var tourTravels = new List<TourTravelDto>();

            var tourGallery = from tg in _dbContext.TourGalleries
                              join g in _dbContext.Galleries on tg.GalleryId equals g.Id
                              where tg.IsDeleted == false
                              select new
                              {
                                  TourId = tg.TourId,
                                  Image = g.Image
                              };
            var tourExclusion = from te in _dbContext.TourExclusions
                              join e in _dbContext.Exclusions on te.ExclusionId equals e.Id
                              where te.IsDeleted == false
                              select new
                              {
                                  TourId = te.TourId,
                                  Name = e.Name
                              };

            var tourInclusion = from ti in _dbContext.TourInclusions
                                join i in _dbContext.Inclusions on ti.InclusionId equals i.Id
                                where ti.IsDeleted == false
                                select new
                                {
                                    TourId = ti.TourId,
                                    Name = i.Name
                                };

            var tourTravelGroupJoin = from t in _dbContext.TourTravels.ToList()
                                      join e in tourExclusion on t.Id equals e.TourId into te
                                      join i in tourInclusion on t.Id equals i.TourId into ti
                                      join s in _dbContext.TourSchedules.ToList() on t.Id equals s.TourId into ts
                                      join p in _dbContext.TourPrices.ToList() on t.Id equals p.TourId into tp
                                      join g in tourGallery on t.Id equals g.TourId into tg
                                      select new
                                      {
                                          TourTravel = t,
                                          Exclusions = te,
                                          Inclusions = ti,
                                          Schedules = ts,
                                          Images = tg,
                                          Prices = tp
                                      };

            foreach (var item in tourTravelGroupJoin)
            {
                var tourTravel = _mapper.Map<TourTravelDto>(item.TourTravel);
                foreach (var ex in item.Exclusions)
                {
                    tourTravel.TourExclusions.Add(ex.Name);
                }

                foreach (var inc in item.Inclusions)
                {
                    tourTravel.TourInclusions.Add(inc.Name);
                }

                foreach (var price in item.Prices)
                {
                    if (price.IsDeleted == false)
                    {
                        var tourPriceMap = _mapper.Map<TourPriceDto>(price);
                        tourTravel.TourPrices.Add(tourPriceMap);
                    }
                }

                foreach (var sche in item.Schedules)
                {
                    if (sche.IsDeleted == false)
                    {
                        var tourScheduleMap = _mapper.Map<TourScheduleDto>(sche);
                        tourTravel.TourSchedules.Add(tourScheduleMap);
                    }
                }

                foreach (var img in item.Images)
                {
                    tourTravel.TourGalleries.Add(img.Image);
                }

                tourTravels.Add(tourTravel);
            }

            return tourTravels;
            

        }

        public TourTravelDto GetByIdDto(int tourTravelId)
        {
            return GetAll().FirstOrDefault(e => e.Id == tourTravelId);
        }

        public bool SetDelete(int id, bool isDelete)
        {
            try
            {
                var selectedRecord = _dbContext.TourTravels.Find(id);
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

       
        public ICollection<TourTravelDto> Search(string field, string keyWords)
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
