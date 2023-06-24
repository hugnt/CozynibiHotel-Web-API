using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;

namespace CozynibiHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomCategoryController : Controller
    {
        private readonly IRoomCategoryService _roomCategoryService;

        public RoomCategoryController(IRoomCategoryService roomCategoryService)
        {
            _roomCategoryService = roomCategoryService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoomCategory>))]
        public IActionResult GetRoomCategories()
        {
            var roomCategories = _roomCategoryService.GetRoomCategories();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(roomCategories);
        }

        [HttpGet("{roomCategoryId}")]
        [ProducesResponseType(200, Type = typeof(RoomCategory))]
        [ProducesResponseType(400)]
        public IActionResult GetRoomCategory(int roomCategoryId)
        {
            var roomCategory = _roomCategoryService.GetRoomCategory(roomCategoryId);
            if (!ModelState.IsValid) return BadRequest();
            if (roomCategory == null) return NotFound();
            return Ok(roomCategory);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateRoomCategory([FromBody] RoomCategoryDto roomCategoryCreate)
        {
            if (roomCategoryCreate == null) return BadRequest(ModelState);

            var res = _roomCategoryService.CreateRoomCategory(roomCategoryCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [HttpPut("{roomCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRoomCategory(int roomCategoryId, [FromBody] RoomCategoryDto updatedRoomCategory)
        {
            if (updatedRoomCategory == null) return BadRequest(ModelState);
            if (roomCategoryId != updatedRoomCategory.Id) return BadRequest(ModelState);

            var res = _roomCategoryService.UpdateRoomCategory(roomCategoryId, updatedRoomCategory);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [HttpDelete("{roomCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoomCategory(int roomCategoryId)
        {
            var res = _roomCategoryService.DeleteRoomCategory(roomCategoryId);
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
