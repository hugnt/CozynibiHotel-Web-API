﻿using AutoMapper;
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
    public class RoomGalleryRepository : GenericRepository<RoomGallery>, IRoomGalleryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public RoomGalleryRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool UpdateStatus(int category_id, List<string> lstUpdateGallery)
        {
            try
            {
                var lstRoomGalleryWithCateId = GetAll().Where(rc => rc.RoomId == category_id);
                foreach (var ri in lstRoomGalleryWithCateId)
                {
                    if (lstUpdateGallery.Contains(ri.Image))
                    {
                        ri.IsDeleted = false;
                    }
                    else
                    {
                        ri.IsDeleted = true;
                    }
                    Update(ri);
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