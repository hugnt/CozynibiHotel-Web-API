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
    public class TourTravelService : ITourTravelService
    {
        private readonly ITourTravelRepository _tourTravelRepository;
        private readonly IMapper _mapper;
        private readonly ITourExclusionRepository _tourExclusionRepository;
        private readonly ITourInclusionRepository _tourInclusionRepository;
        private readonly ITourGalleryRepository _tourGalleryRepository;
        private readonly ITourScheduleRepository _tourScheduleRepository;
        private readonly ITourPriceRepository _tourPriceRepository;
        private readonly IGalleryRepository _galleryRepository;
        private readonly IExclusionRepository _exclusionRepository;
        private readonly IInclusionRepository _inclusionRepository;


        public TourTravelService(ITourTravelRepository tourTravelRepository, 
                                   IMapper mapper,
                                   ITourExclusionRepository tourExclusionRepository,
                                   ITourInclusionRepository tourInclusionRepository,
                                   ITourGalleryRepository tourGalleryRepository,
                                   IGalleryRepository galleryRepository,
                                   ITourScheduleRepository tourScheduleRepository,
                                   ITourPriceRepository tourPriceRepository,
                                   IExclusionRepository exclusionRepository,
                                   IInclusionRepository inclusionRepository
                                   )
        {
            _tourTravelRepository = tourTravelRepository;
            _mapper = mapper;
            _tourExclusionRepository = tourExclusionRepository;
            _tourInclusionRepository = tourInclusionRepository;
            _tourGalleryRepository = tourGalleryRepository;
            _galleryRepository = galleryRepository;
            _tourScheduleRepository = tourScheduleRepository;
            _tourPriceRepository = tourPriceRepository;
            _exclusionRepository = exclusionRepository;
            _inclusionRepository = inclusionRepository;
        }

        public TourTravelDto GetTourTravel(int tourTravelId)
        {
            if (!_tourTravelRepository.IsExists(tourTravelId)) return null;
            var tourTravel = _tourTravelRepository.GetByIdDto(tourTravelId);
            return tourTravel;
        }
        public IEnumerable<TourTravelDto> GetTourTravels()
        {
            return _tourTravelRepository.GetAll();
        }
        public ResponseModel CreateTourTravel(TourTravelDto tourTravelCreate)
        {
            if (tourTravelCreate.CreatedBy == 0) tourTravelCreate.CreatedBy = 1;
            if (tourTravelCreate.UpdatedBy == 0) tourTravelCreate.UpdatedBy = 1;
            tourTravelCreate.CreatedAt = DateTime.Now;
            tourTravelCreate.IsActive = false;
            tourTravelCreate.IsDeleted = false;
            var tourTravels = _tourTravelRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == tourTravelCreate.Name.Trim().ToLower())
                            .FirstOrDefault();
            if (tourTravels != null)
            {
                return new ResponseModel(422, "TourTravel already exists");
            }

            
            var tourTravelMap = _mapper.Map<TourTravel>(tourTravelCreate);
            tourTravelMap.CreatedAt = DateTime.Now;


            if (!_tourTravelRepository.Create(tourTravelMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            foreach (var ex in tourTravelCreate.TourExclusions)
            {
                //Create || Checking exist
                var checkExclusionExist = _exclusionRepository.GetAll().FirstOrDefault(e =>
                                      e.Name == ex);
                if (checkExclusionExist == null)
                {
                    var newExclusion = new Exclusion()
                    {
                        Name = ex,
                        CreatedBy = tourTravelCreate.CreatedBy,
                        UpdatedBy = tourTravelCreate.UpdatedBy
                    };
                    if (!_exclusionRepository.Create(newExclusion))
                    {
                        return new ResponseModel(500, "Something went wrong while adding new exclusion");
                    }
                    checkExclusionExist = newExclusion;
                }

                //Create relation ship
                var tourExclusion= new TourExclusion()
                {
                    TourId = tourTravelMap.Id,
                    ExclusionId = checkExclusionExist.Id,
                    CreatedBy = tourTravelCreate.CreatedBy,
                    UpdatedBy = tourTravelCreate.UpdatedBy,
                    IsDeleted = false

                };
                var checkTourExclusionExist = _tourExclusionRepository.GetAll().Any(tourEx =>
                                                                    tourEx.TourId == tourExclusion.TourId &&
                                                                    tourEx.ExclusionId == tourExclusion.ExclusionId);
                if (checkTourExclusionExist) continue;
                if (!_tourExclusionRepository.Create(tourExclusion))
                {
                    return new ResponseModel(500, "Something went wrong while saving exclusion");
                }
            }

            foreach (var inc in tourTravelCreate.TourInclusions)
            {
                //Create || Checking exist
                var checkInclusionExist = _inclusionRepository.GetAll().FirstOrDefault(e =>
                                      e.Name == inc);
                if (checkInclusionExist == null)
                {
                    var newInclusion = new Inclusion()
                    {
                        Name = inc,
                        CreatedBy = tourTravelCreate.CreatedBy,
                        UpdatedBy = tourTravelCreate.UpdatedBy
                    };
                    if (!_inclusionRepository.Create(newInclusion))
                    {
                        return new ResponseModel(500, "Something went wrong while adding new inclusion");
                    }
                    checkInclusionExist = newInclusion;
                }

                //Create relation ship
                var tourInclusion = new TourInclusion()
                {
                    TourId = tourTravelMap.Id,
                    InclusionId = checkInclusionExist.Id,
                    CreatedBy = tourTravelCreate.CreatedBy,
                    UpdatedBy = tourTravelCreate.UpdatedBy,
                    IsDeleted = false

                };
                var checkTourInclusionExist = _tourInclusionRepository.GetAll().Any(tourEx =>
                                                                    tourEx.TourId == tourInclusion.TourId &&
                                                                    tourEx.InclusionId == tourInclusion.InclusionId);
                if (checkTourInclusionExist) continue;
                if (!_tourInclusionRepository.Create(tourInclusion))
                {
                    return new ResponseModel(500, "Something went wrong while saving inclusion");
                }
            }

            foreach (var price in tourTravelCreate.TourPrices)
            {
                var tourPrice = new TourPrice()
                {
                    TourId = tourTravelMap.Id,
                    Price = price.Price,
                    MinPeople = price.MinPeople,
                    MaxPeople = price.MaxPeople,
                    CreatedBy = tourTravelCreate.CreatedBy,
                    UpdatedBy = tourTravelCreate.UpdatedBy,
                    IsDeleted = false
                };
                var checkPriceExist = _tourPriceRepository.GetAll().Any(pri =>
                                                                    pri.TourId == tourPrice.TourId &&
                                                                    pri.MinPeople == tourPrice.MinPeople &&
                                                                    pri.MaxPeople == tourPrice.MaxPeople
                                                                    );
                if (checkPriceExist) continue;
                if (!_tourPriceRepository.Create(tourPrice))
                {
                    return new ResponseModel(500, "Something went wrong while saving tour prices");
                }
            }

            foreach (var sche in tourTravelCreate.TourSchedules)
            {
                var tourSchedule = new TourSchedule()
                {
                    TourId = tourTravelMap.Id,
                    StartTime = sche.StartTime,
                    EndTime = sche.EndTime,
                    Content = sche.Content,
                    CreatedBy = tourTravelCreate.CreatedBy,
                    UpdatedBy = tourTravelCreate.UpdatedBy,
                    IsDeleted = false
                };
                var checkScheduleExist = _tourScheduleRepository.GetAll().Any(sche =>
                                                                    sche.TourId == tourSchedule.TourId &&
                                                                    sche.StartTime == tourSchedule.StartTime &&
                                                                    sche.EndTime == tourSchedule.EndTime &&
                                                                    sche.Content == tourSchedule.Content
                                                                    );
                if (checkScheduleExist) continue;
                if (!_tourScheduleRepository.Create(tourSchedule))
                {
                    return new ResponseModel(500, "Something went wrong while saving tour schedules");
                }
            }

            foreach (var img in tourTravelCreate.TourGalleries)
            {
                //Create || Checking exist
                var checkImageExist = _galleryRepository.GetAll().FirstOrDefault(g =>
                                      g.Image == img);
                if (checkImageExist == null)
                {
                    var newImage = new Gallery()
                    {
                        Image = img,
                        CreatedBy = tourTravelCreate.CreatedBy,
                        UpdatedBy = tourTravelCreate.UpdatedBy
                    };
                    if (!_galleryRepository.Create(newImage))
                    {
                        return new ResponseModel(500, "Something went wrong while adding new images");
                    }
                    checkImageExist = newImage;
                }

              //Create relation ship
              var tourGallery = new TourGallery()
                {
                    TourId = tourTravelMap.Id,
                    GalleryId = checkImageExist.Id,
                    CreatedBy = tourTravelCreate.CreatedBy,
                    UpdatedBy = tourTravelCreate.UpdatedBy,
                    IsDeleted = false

              };
                var checkTourImageExist = _tourGalleryRepository.GetAll().Any(tourGal =>
                                                                    tourGal.TourId == tourGallery.TourId &&
                                                                    tourGal.GalleryId == tourGallery.GalleryId);
                if (checkTourImageExist) continue;
                if (!_tourGalleryRepository.Create(tourGallery))
                {
                    return new ResponseModel(500, "Something went wrong while saving images");
                }
            }


            return new ResponseModel(201, "Successfully created");

        }

        public ResponseModel UpdateTourTravel(int tourTravelId, TourTravelDto updatedTourTravel)
        {
            if (updatedTourTravel.CreatedBy == 0) updatedTourTravel.CreatedBy = 1;
            if (updatedTourTravel.UpdatedBy == 0) updatedTourTravel.UpdatedBy = 1;
            updatedTourTravel.UpdatedAt = DateTime.Now;

            if (!_tourTravelRepository.IsExists(tourTravelId)) return new ResponseModel(404,"Not found");
            var tourTravelMap = _mapper.Map<TourTravel>(updatedTourTravel);
            if (!_tourTravelRepository.Update(tourTravelMap))
            {
                return new ResponseModel(500, "Something went wrong updating tourTravel");
            }

            //Othes
            //Reset 
            if (!_tourExclusionRepository.SetDeletedAll(tourTravelId))
            {
                return new ResponseModel(500, "Something went wrong reseting status of inclusion tourExclusion");
            }
            foreach (var ex in updatedTourTravel.TourExclusions)
            {
                //Create || Checking exist
                var checkExclusionExist = _exclusionRepository.GetAll().FirstOrDefault(e =>
                                      e.Name == ex);
                if (checkExclusionExist == null)
                {
                    var newExclusion = new Exclusion()
                    {
                        Name = ex,
                        CreatedBy = updatedTourTravel.CreatedBy,
                        UpdatedBy = updatedTourTravel.UpdatedBy
                    };
                    if (!_exclusionRepository.Create(newExclusion))
                    {
                        return new ResponseModel(500, "Something went wrong while adding new exclusion");
                    }
                    checkExclusionExist = newExclusion;
                }

                //Create relation ship
                var tourExclusion = new TourExclusion()
                {
                    TourId = tourTravelMap.Id,
                    ExclusionId = checkExclusionExist.Id,
                    CreatedBy = updatedTourTravel.CreatedBy,
                    UpdatedBy = updatedTourTravel.UpdatedBy,
                    IsDeleted = false

                };
                var checkTourExclusionExist = _tourExclusionRepository.GetAll().Any(tourEx =>
                                                                    tourEx.TourId == tourExclusion.TourId &&
                                                                    tourEx.ExclusionId == tourExclusion.ExclusionId);
                if (checkTourExclusionExist) 
                {
                    if (!_tourExclusionRepository.Update(tourExclusion))
                    {
                        return new ResponseModel(500, "Something went wrong while updating exclusion");
                    }
                    continue;
                }
                if (!_tourExclusionRepository.Create(tourExclusion))
                {
                    return new ResponseModel(500, "Something went wrong while saving exclusion");
                }
            }

            if (!_tourInclusionRepository.SetDeletedAll(tourTravelId))
            {
                return new ResponseModel(500, "Something went wrong reseting status of inclusion tourInclusion");
            }
            foreach (var inc in updatedTourTravel.TourInclusions)
            {
                //Create || Checking exist
                var checkInclusionExist = _inclusionRepository.GetAll().FirstOrDefault(e =>
                                      e.Name == inc);
                if (checkInclusionExist == null)
                {
                    var newInclusion = new Inclusion()
                    {
                        Name = inc,
                        CreatedBy = updatedTourTravel.CreatedBy,
                        UpdatedBy = updatedTourTravel.UpdatedBy
                    };
                    if (!_inclusionRepository.Create(newInclusion))
                    {
                        return new ResponseModel(500, "Something went wrong while adding new inclusion");
                    }
                    checkInclusionExist = newInclusion;
                }

                //Create relation ship
                var tourInclusion = new TourInclusion()
                {
                    TourId = tourTravelMap.Id,
                    InclusionId = checkInclusionExist.Id,
                    CreatedBy = updatedTourTravel.CreatedBy,
                    UpdatedBy = updatedTourTravel.UpdatedBy,
                    IsDeleted = false

                };
                var checkTourInclusionExist = _tourInclusionRepository.GetAll().Any(tourEx =>
                                                                    tourEx.TourId == tourInclusion.TourId &&
                                                                    tourEx.InclusionId == tourInclusion.InclusionId);
                if (checkTourInclusionExist)
                {
                    if (!_tourInclusionRepository.Update(tourInclusion))
                    {
                        return new ResponseModel(500, "Something went wrong while updating inclusion");
                    }
                    continue;
                }
                if (!_tourInclusionRepository.Create(tourInclusion))
                {
                    return new ResponseModel(500, "Something went wrong while saving inclusion");
                }
            }

            if (!_tourPriceRepository.SetDeletedAll(tourTravelId))
            {
                return new ResponseModel(500, "Something went wrong reseting status of inclusion tourPrice");
            }
            foreach (var price in updatedTourTravel.TourPrices)
            {
                var tourPrice = new TourPrice()
                {
                    TourId = tourTravelMap.Id,
                    Price = price.Price,
                    MinPeople = price.MinPeople,
                    MaxPeople = price.MaxPeople,
                    CreatedBy = updatedTourTravel.CreatedBy,
                    UpdatedBy = updatedTourTravel.UpdatedBy,
                    IsDeleted = false
                };
                var checkPriceExist = _tourPriceRepository.GetAll().Any(pri =>
                                                                    pri.TourId == tourPrice.TourId &&
                                                                    pri.MinPeople == tourPrice.MinPeople &&
                                                                    pri.MaxPeople == tourPrice.MaxPeople
                                                                    );
                if (checkPriceExist)
                {
                    if (!_tourPriceRepository.Update(tourPrice))
                    {
                        return new ResponseModel(500, "Something went wrong while updating price relationship");
                    }
                    continue;
                }
                if (!_tourPriceRepository.Create(tourPrice))
                {
                    return new ResponseModel(500, "Something went wrong while saving tour prices");
                }
            }

            if (!_tourScheduleRepository.SetDeletedAll(tourTravelId))
            {
                return new ResponseModel(500, "Something went wrong reseting status of schedule tourTravel");
            }
            foreach (var sche in updatedTourTravel.TourSchedules)
            {
                var tourSchedule = new TourSchedule()
                {
                    TourId = tourTravelMap.Id,
                    StartTime = sche.StartTime,
                    EndTime = sche.EndTime,
                    Content = sche.Content,
                    CreatedBy = updatedTourTravel.CreatedBy,
                    UpdatedBy = updatedTourTravel.UpdatedBy,
                    IsDeleted = false
                };
                var checkScheduleExist = _tourScheduleRepository.GetAll().Any(sche =>
                                                                    sche.TourId == tourSchedule.TourId &&
                                                                    sche.StartTime == tourSchedule.StartTime &&
                                                                    sche.EndTime == tourSchedule.EndTime &&
                                                                    sche.Content == tourSchedule.Content
                                                                    );
                if (checkScheduleExist)
                {
                    if (!_tourScheduleRepository.Update(tourSchedule))
                    {
                        return new ResponseModel(500, "Something went wrong while updating tourSchedule");
                    }
                    continue;
                }
                if (!_tourScheduleRepository.Create(tourSchedule))
                {
                    return new ResponseModel(500, "Something went wrong while saving tour inclusions");
                }
            }

            if (!_tourGalleryRepository.SetDeletedAll(tourTravelId))
            {
                return new ResponseModel(500, "Something went wrong reseting status of schedule tourTravel");
            }
            foreach (var img in updatedTourTravel.TourGalleries)
            {
                //Create || Checking exist
                var checkImageExist = _galleryRepository.GetAll().FirstOrDefault(g =>
                                      g.Image == img);
                if (checkImageExist == null)
                {
                    var newImage = new Gallery()
                    {
                        Image = img,
                        CreatedBy = updatedTourTravel.CreatedBy,
                        UpdatedBy = updatedTourTravel.UpdatedBy
                    };
                    if (!_galleryRepository.Create(newImage))
                    {
                        return new ResponseModel(500, "Something went wrong while adding new equipment");
                    }
                    checkImageExist = newImage;
                }

                //Create relation ship
                var tourGallery = new TourGallery()
                {
                    TourId = tourTravelMap.Id,
                    GalleryId = checkImageExist.Id,
                    CreatedBy = updatedTourTravel.CreatedBy,
                    UpdatedBy = updatedTourTravel.UpdatedBy,
                    IsDeleted = false

                };
                var checkTourImageExist = _tourGalleryRepository.GetAll().Any(tourGal =>
                                                                    tourGal.TourId == tourGallery.TourId &&
                                                                    tourGal.GalleryId == tourGallery.GalleryId);
                if (checkTourImageExist)
                {
                    if (!_tourGalleryRepository.Update(tourGallery))
                    {
                        return new ResponseModel(500, "Something went wrong while updating tourGallery");
                    }
                    continue;
                }
                if (!_tourGalleryRepository.Create(tourGallery))
                {
                    return new ResponseModel(500, "Something went wrong while saving images");
                }
            }

            return new ResponseModel(204, "");

        }


        public ResponseModel UpdateTourTravel(int tourTravelId, bool isDelete)
        {
            if(!_tourTravelRepository.SetDelete(tourTravelId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete tourTravel");
            }
            return new ResponseModel(204, "");
        }

        public ResponseModel DeleteTourTravel(int tourTravelId)
        {
            if (!_tourTravelRepository.IsExists(tourTravelId)) return new ResponseModel(404, "Not found");
            var tourTravelToDelete = _tourTravelRepository.GetById(tourTravelId);
            if (!_tourTravelRepository.Delete(tourTravelToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting tourTravel");
            }

            return new ResponseModel(204, "");
        }

        public IEnumerable<TourTravelDto> SearchTourTravels(string field, string keyWords)
        {
            var res = _tourTravelRepository.Search(field, keyWords);
            return res;
        }
    }
}
