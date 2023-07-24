using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HUG.CRUD.Services;
using CozynibiHotel.API.Hub;
using Microsoft.AspNetCore.SignalR;
using HUG.EmailServices.Services;
using HUG.EmailServices.Models;
using HUG.QRCodeServices.Services;
using Microsoft.Extensions.Options;
using CozynibiHotel.API.Models;
using HUG.QRCodeServices.Models;

namespace CozynibiHotel.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IWebHostEnvironment _environment;
        private IHubContext<MessageHub, IMessageHubClient> _messageHub;
        private readonly EmailSettings _emailSettings;


        public BookingController(IBookingService bookingService,
                                IWebHostEnvironment environment,
                                IHubContext<MessageHub, IMessageHubClient> messageHub,
                                IOptions<EmailSettings> options)
        {
            _bookingService = bookingService;
            _environment = environment;
            _messageHub = messageHub;
            _emailSettings = options.Value;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Booking>))]
        public IActionResult GetBookings()
        {
            var bookings = _bookingService.GetBookings();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(bookings);
        }

        [Authorize]
        [HttpGet("{bookingId}")]
        [ProducesResponseType(200, Type = typeof(Booking))]
        [ProducesResponseType(400)]
        public IActionResult GetBooking(int bookingId)
        {
            var booking = _bookingService.GetBooking(bookingId);
            if (!ModelState.IsValid) return BadRequest();
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        [Authorize]
        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookingDto>))]
        public IActionResult SearchBookingCategories(string field, string keyWords)
        {
            var bookingCategories = _bookingService.SearchBookings(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (bookingCategories == null) return NotFound();
            return Ok(bookingCategories);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingCreate)
        {

            if (bookingCreate == null) return BadRequest(ModelState);
            var res = _bookingService.CreateBooking(bookingCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            _messageHub.Clients.All.SendNotificationBooking(bookingCreate);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [Authorize]
        [HttpPut("{bookingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBooking(int bookingId, [FromForm] BookingDto updatedBooking)
        {
            if (updatedBooking == null) return BadRequest(ModelState);
            if (bookingId != updatedBooking.Id) return BadRequest(ModelState);

            var res = _bookingService.UpdateBooking(bookingId, updatedBooking);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{bookingId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBooking(int bookingId, bool isDelete)
        {

            var res = _bookingService.UpdateBooking(bookingId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPut("{bookingId}/Status/{status}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateBookingStatus(int bookingId, bool status)
        {

            var res = _bookingService.UpdateBookingStatus(bookingId, status);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }


        [Authorize]
        [HttpDelete("{bookingId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBooking(int bookingId)
        {
            var res = _bookingService.DeleteBooking(bookingId);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [Authorize]
        [HttpPut("ConfirmBooking/{bookingId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConfirmBooking(int bookingId)
        {

            var res = await _bookingService.ConfirmBooking(bookingId, _emailSettings);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [Authorize]
        [HttpPost("ValidateQRBooking")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookingDto>))]
        [ProducesResponseType(400)]
        public IActionResult ValidateQRBooking([FromForm]string qrToken)
        {

            var res =  _bookingService.ValidateQRBooking(qrToken);
            if (!ModelState.IsValid) return BadRequest();
            if (res == null) return NotFound();
            return Ok(res);
        }







    }
}
