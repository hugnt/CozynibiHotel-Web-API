using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CozynibiHotel.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Room>))]
        public IActionResult GetRooms()
        {
            var rooms = _roomService.GetRooms();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(rooms);
        }

        [HttpGet("{roomId}")]
        [ProducesResponseType(200, Type = typeof(Room))]
        [ProducesResponseType(400)]
        public IActionResult GetRoom(int roomId)
        {
            var room = _roomService.GetRoom(roomId);
            if (!ModelState.IsValid) return BadRequest();
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateRoom([FromBody] RoomDto roomCreate)
        {
            if (roomCreate == null) return BadRequest(ModelState);

            var res = _roomService.CreateRoom(roomCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [HttpPut("{roomId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRoom(int roomId, [FromBody] RoomDto updatedRoom)
        {
            if (updatedRoom == null) return BadRequest(ModelState);
            if (roomId != updatedRoom.Id) return BadRequest(ModelState);

            var res = _roomService.UpdateRoom(roomId, updatedRoom);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [HttpDelete("{roomId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoom(int roomId)
        {
            var res = _roomService.DeleteRoom(roomId);
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
