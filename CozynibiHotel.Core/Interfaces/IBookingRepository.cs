using CozynibiHotel.Core.Dto;
using CozynibiHotel.Core.Models;
using HUG.CRUD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Core.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        ICollection<BookingDto> Search(string field, string keyWords);
        bool SetDelete(int id, bool isDelete);
        public bool SetStatus(int id, bool status);
        public bool UpdateCheckInCode(int bookingId, int checkInCode);
    }
}
