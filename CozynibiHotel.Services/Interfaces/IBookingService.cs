using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using HUG.EmailServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<BookingDto> GetBookings();
        IEnumerable<BookingDto> SearchBookings(string field, string keyWords);
        BookingDto GetBooking(int bookingId);
        ResponseModel CreateBooking(BookingDto bookingCreate);
        ResponseModel UpdateBooking(int bookingId, BookingDto updatedBooking);
        ResponseModel UpdateBooking(int bookingId, bool isDelete);
        ResponseModel UpdateBookingStatus(int bookingId, bool status);
        ResponseModel DeleteBooking(int bookingCategoryId);
        Task<ResponseModel> ConfirmBooking(int bookingId, EmailSettings emailSettings);
        public BookingDto ValidateQRBooking(string qrToken);
    }
}
