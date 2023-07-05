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
    public class RoomEquipmentRepository : GenericRepository<RoomEquipment>, IRoomEquipmentRepository
    {
        private readonly AppDbContext _dbContext;
        public RoomEquipmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool UpdateStatus(int category_id, List<string> lstUpdateEquipments)
        {
            try
            {
                var lstEquipmentId = new List<int>();

                foreach (var e in _dbContext.Equipments)
                {
                    if (lstUpdateEquipments.Contains(e.Name))
                    {
                        lstEquipmentId.Add(e.Id);
                    }
                }
                
                var lstRoomEquipmentWithCateId = GetAll().Where(rc => rc.CategoryId == category_id);
                foreach (var re in lstRoomEquipmentWithCateId)
                {
                    if (lstEquipmentId.Contains(re.EquipmentId))
                    {
                        re.IsDeleted = false;
                    }
                    else
                    {
                        re.IsDeleted = true;
                    }
                    Update(re);
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
