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
    public class RoomCategoryController : Controller
    {
        private readonly IRoomCategoryRepository _roomCategoryRepository;
        private readonly IMapper _mapper;

        public RoomCategoryController(IRoomCategoryRepository roomCategoryRepository, IMapper mapper)
        {
            _roomCategoryRepository = roomCategoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoomCategory>))]
        public IActionResult GetRoomCategories()
        {
            var roomCategories = _mapper.Map<List<RoomCategoryDto>>(_roomCategoryRepository.GetAll());
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(roomCategories);
        }

        [HttpGet("{roomCategoryId}")]
        [ProducesResponseType(200, Type = typeof(RoomCategory))]
        [ProducesResponseType(400)]
        public IActionResult GetRoomCategory(int roomCategoryId)
        {
            if (!_roomCategoryRepository.IsExists(roomCategoryId)) return NotFound();

            var roomCategory = _mapper.Map<RoomCategoryDto>(_roomCategoryRepository.GetById(roomCategoryId));
            if (!ModelState.IsValid) return BadRequest();

            return Ok(roomCategory);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRoomCategory([FromBody] RoomCategoryDto roomCategoryCreate)
        {
            if (roomCategoryCreate == null) return BadRequest(ModelState);

            var roomCategories = _roomCategoryRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == roomCategoryCreate.Name.Trim().ToLower())
                            .FirstOrDefault();

            if (roomCategories != null)
            {
                ModelState.AddModelError("", "RoomCategory already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var roomCategoryMap = _mapper.Map<RoomCategory>(roomCategoryCreate);

            if (!_roomCategoryRepository.Create(roomCategoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{roomCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRoomCategory(int roomCategoryId, [FromBody] RoomCategoryDto updatedRoomCategory)
        {
            if (updatedRoomCategory == null) return BadRequest(ModelState);
            if (roomCategoryId != updatedRoomCategory.Id) return BadRequest(ModelState);
            if (!_roomCategoryRepository.IsExists(roomCategoryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var roomCategoryMap = _mapper.Map<RoomCategory>(updatedRoomCategory);

            if (!_roomCategoryRepository.Update(roomCategoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating roomCategory");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{roomCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoomCategory(int roomCategoryId)
        {
            if (!_roomCategoryRepository.IsExists(roomCategoryId)) return NotFound();

            var roomCategoryToDelete = _roomCategoryRepository.GetById(roomCategoryId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_roomCategoryRepository.Delete(roomCategoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting roomCategory");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



    }
}
