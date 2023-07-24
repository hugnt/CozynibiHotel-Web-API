using CozynibiHotel.Core.Dto;

namespace CozynibiHotel.API.Hub
{
    public interface IMessageHubClient
    {
        Task SendOffersToUser(List<string> message);
        Task SendNotificationToUser(ContactDto contact);
        Task SendNotificationBooking(BookingDto booking);
    }
}
