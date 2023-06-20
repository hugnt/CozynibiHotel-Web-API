using AutoMapper;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CozynibiHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomController(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Room>))]
        public IActionResult GetRooms()
        {
            var rooms = _mapper.Map<List<RoomDto>>(_roomRepository.GetAll());
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(rooms);
        }

        [HttpGet("{roomId}")]
        [ProducesResponseType(200, Type = typeof(Room))]
        [ProducesResponseType(400)]
        public IActionResult GetRoom(int roomId)
        {
            if (!_roomRepository.IsExists(roomId)) return NotFound();

            var room = _mapper.Map<RoomDto>(_roomRepository.GetById(roomId));
            if (!ModelState.IsValid) return BadRequest();

            return Ok(room);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRoom([FromBody] RoomDto roomCreate)
        {
            if (roomCreate == null) return BadRequest(ModelState);

            var rooms = _roomRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == roomCreate.Name.Trim().ToLower())
                            .FirstOrDefault();

            if (rooms != null)
            {
                ModelState.AddModelError("", "Room already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var roomMap = _mapper.Map<Room>(roomCreate);

            if (!_roomRepository.Create(roomMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{roomId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRoom(int roomId, [FromBody] RoomDto updatedRoom)
        {
            if (updatedRoom == null) return BadRequest(ModelState);
            if (roomId != updatedRoom.Id) return BadRequest(ModelState);
            if (!_roomRepository.IsExists(roomId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var roomMap = _mapper.Map<Room>(updatedRoom);

            if (!_roomRepository.Update(roomMap))
            {
                ModelState.AddModelError("", "Something went wrong updating room");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{roomId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoom(int roomId)
        {
            if (!_roomRepository.IsExists(roomId)) return NotFound();

            var roomToDelete = _roomRepository.GetById(roomId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_roomRepository.Delete(roomToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting room");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



    }
}
