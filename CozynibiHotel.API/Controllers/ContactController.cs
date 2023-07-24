using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HUG.CRUD.Services;
using CozynibiHotel.API.Hub;
using Microsoft.AspNetCore.SignalR;

namespace CozynibiHotel.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IWebHostEnvironment _environment;
        private IHubContext<MessageHub, IMessageHubClient> _messageHub;

        public ContactController(IContactService contactService,
                                IWebHostEnvironment environment,
                                IHubContext<MessageHub, IMessageHubClient> messageHub
                                )
        {
            _contactService = contactService;
            _environment = environment;
            _messageHub = messageHub;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Contact>))]
        public IActionResult GetContacts()
        {
            var contacts = _contactService.GetContacts();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(contacts);
        }

        [Authorize]
        [HttpGet("{contactId}")]
        [ProducesResponseType(200, Type = typeof(Contact))]
        [ProducesResponseType(400)]
        public IActionResult GetContact(int contactId)
        {
            var contact = _contactService.GetContact(contactId);
            if (!ModelState.IsValid) return BadRequest();
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        [Authorize]
        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ContactDto>))]
        public IActionResult SearchContactCategories(string field, string keyWords)
        {
            var contactCategories = _contactService.SearchContacts(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (contactCategories == null) return NotFound();
            return Ok(contactCategories);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateContact([FromBody] ContactDto contactCreate)
        {

            if (contactCreate == null) return BadRequest(ModelState);
            var res = _contactService.CreateContact(contactCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            _messageHub.Clients.All.SendNotificationToUser(contactCreate);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [Authorize]
        [HttpPut("{contactId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateContact(int contactId, [FromForm] ContactDto updatedContact)
        {
            if (updatedContact == null) return BadRequest(ModelState);
            if (contactId != updatedContact.Id) return BadRequest(ModelState);

            var res = _contactService.UpdateContact(contactId, updatedContact);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{contactId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateContact(int contactId, bool isDelete)
        {

            var res = _contactService.UpdateContact(contactId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPut("{contactId}/Status/{status}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateContactStatus(int contactId, bool status)
        {

            var res = _contactService.UpdateContactStatus(contactId, status);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }


        [Authorize]
        [HttpDelete("{contactId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteContact(int contactId)
        {
            var res = _contactService.DeleteContact(contactId);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

       


    }
}
