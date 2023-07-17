using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HUG.CRUD.Services;

namespace CozynibiHotel.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CustommerController : Controller
    {
        private readonly ICustommerService _custommerService;
        private readonly IWebHostEnvironment _environment;

        public CustommerController(ICustommerService custommerService, IWebHostEnvironment environment)
        {
            _custommerService = custommerService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Custommer>))]
        public IActionResult GetCustommers()
        {
            var custommers = _custommerService.GetCustommers();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(custommers);
        }

        [HttpGet("{custommerId}")]
        [ProducesResponseType(200, Type = typeof(Custommer))]
        [ProducesResponseType(400)]
        public IActionResult GetCustommer(int custommerId)
        {
            var custommer = _custommerService.GetCustommer(custommerId);
            if (!ModelState.IsValid) return BadRequest();
            if (custommer == null) return NotFound();
            return Ok(custommer);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCustommer([FromForm] CustommerDto custommerCreate, [FromForm] List<IFormFile> images)
        {
            if (custommerCreate == null) return BadRequest(ModelState);

            var res = _custommerService.CreateCustommer(custommerCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\custommer";
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

        [Authorize]
        [HttpPut("{custommerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCustommer(int custommerId, [FromForm] CustommerDto updatedCustommer, [FromForm] List<IFormFile> images)
        {
            if (updatedCustommer == null) return BadRequest(ModelState);
            if (custommerId != updatedCustommer.Id) return BadRequest(ModelState);

            var res = _custommerService.UpdateCustommer(custommerId, updatedCustommer);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\custommer";
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

        [Authorize]
        [HttpPut("{custommerId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCustommer(int custommerId, bool isDelete)
        {

            var res = _custommerService.UpdateCustommer(custommerId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{custommerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCustommer(int custommerId)
        {
            var res = _custommerService.DeleteCustommer(custommerId);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [Authorize]
        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CustommerDto>))]
        public IActionResult SearchCustommerCategories(string field, string keyWords)
        {
            var custommerCategories = _custommerService.SearchCustommers(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (custommerCategories == null) return NotFound();
            return Ok(custommerCategories);
        }


    }
}
