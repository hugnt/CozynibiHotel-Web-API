using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using HUG.CRUD.Services;
using Microsoft.AspNetCore.Authorization;

namespace CozynibiHotel.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomCategoryController : Controller
    {
        private readonly IRoomCategoryService _roomCategoryService;
        private readonly IWebHostEnvironment _environment;

        public RoomCategoryController(IRoomCategoryService roomCategoryService, IWebHostEnvironment environment)
        {
            _roomCategoryService = roomCategoryService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoomCategory>))]
        public IActionResult GetRoomCategories()
        {
            var roomCategories = _roomCategoryService.GetRoomCategories();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(roomCategories);
        }

        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RoomCategoryDto>))]
        public IActionResult SearchRoomCategories(string field, string keyWords)
        {
            var roomCategories = _roomCategoryService.SearchRoomCategories(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (roomCategories == null) return NotFound();
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
        public async Task<IActionResult> CreateRoomCategory([FromForm] RoomCategoryDto roomCategoryCreate, [FromForm] List<IFormFile> images)
        {
            if (roomCategoryCreate == null) return BadRequest(ModelState);

            var res = _roomCategoryService.CreateRoomCategory(roomCategoryCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\accommodation_2";
            var uploadFile = new UploadFile(_environment.WebRootPath);
            var resUploadImage = await uploadFile.UploadImage(images, folderImage);

            if (resUploadImage.Status != 200)
            {
                ModelState.AddModelError("", resUploadImage.StatusMessage);
                return StatusCode(resUploadImage.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [HttpPut("{roomCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRoomCategory(int roomCategoryId, [FromForm] RoomCategoryDto updatedRoomCategory, [FromForm] List<IFormFile> images)
        {
            if (updatedRoomCategory == null) return BadRequest(ModelState);
            if (roomCategoryId != updatedRoomCategory.Id) return BadRequest(ModelState);

            var res = _roomCategoryService.UpdateRoomCategory(roomCategoryId, updatedRoomCategory);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\accommodation_2";
            var uploadFile = new UploadFile(_environment.WebRootPath);
            var resUploadImage = await uploadFile.UploadImage(images, folderImage);

            if (resUploadImage.Status != 200)
            {
                ModelState.AddModelError("", resUploadImage.StatusMessage);
                return StatusCode(resUploadImage.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [HttpPut("{roomCategoryId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRoomCategory(int roomCategoryId, bool isDelete)
        {

            var res = _roomCategoryService.UpdateRoomCategory(roomCategoryId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

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
