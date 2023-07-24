using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.SignalR;

namespace CozynibiHotel.API.Hub
{
    public class MessageHub : Hub<IMessageHubClient> 
    {
        public async Task SendOffersToUser(List<string> message)
        {
            await Clients.All.SendOffersToUser(message);
        }
        public async Task SendNotificationToUser(ContactDto contact)
        {
            await Clients.All.SendNotificationToUser(contact);
        }
        public async Task SendNotificationBooking(BookingDto booking)
        {
            await Clients.All.SendNotificationBooking(booking);
        }

    }
}
