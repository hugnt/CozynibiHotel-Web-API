using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using HUG.CRUD.Services;
using Microsoft.AspNetCore.Authorization;

namespace CozynibiHotel.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class NewsCategoryController : Controller
    {
        private readonly INewsCategoryService _newsCategoryService;
        private readonly IWebHostEnvironment _environment;

        public NewsCategoryController(INewsCategoryService newsCategoryService, IWebHostEnvironment environment)
        {
            _newsCategoryService = newsCategoryService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsCategoryDto>))]
        public IActionResult GetNewsCategories()
        {
            var newsCategories = _newsCategoryService.GetNewsCategories();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(newsCategories);
        }

        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsCategoryDto>))]
        public IActionResult SearchNewsCategories(string field, string keyWords)
        {
            var newsCategories = _newsCategoryService.SearchNewsCategories(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (newsCategories == null) return NotFound();
            return Ok(newsCategories);
        }

        [HttpGet("{newsCategoryId}")]
        [ProducesResponseType(200, Type = typeof(NewsCategory))]
        [ProducesResponseType(400)]
        public IActionResult GetNewsCategory(int newsCategoryId)
        {
            var newsCategory = _newsCategoryService.GetNewsCategory(newsCategoryId);
            if (!ModelState.IsValid) return BadRequest();
            if (newsCategory == null) return NotFound();
            return Ok(newsCategory);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateNewsCategory([FromForm] NewsCategoryDto newsCategoryCreate, [FromForm] List<IFormFile> images)
        {
            if (newsCategoryCreate == null) return BadRequest(ModelState);

            var res = _newsCategoryService.CreateNewsCategory(newsCategoryCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\news";
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
        [HttpPut("{newsCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateNewsCategory(int newsCategoryId, [FromForm] NewsCategoryDto updatedNewsCategory, [FromForm] List<IFormFile> images)
        {
            if (updatedNewsCategory == null) return BadRequest(ModelState);
            if (newsCategoryId != updatedNewsCategory.Id) return BadRequest(ModelState);

            var res = _newsCategoryService.UpdateNewsCategory(newsCategoryId, updatedNewsCategory);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\news";
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
        [HttpPut("{newsCategoryId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateNewsCategory(int newsCategoryId, bool isDelete)
        {

            var res = _newsCategoryService.UpdateNewsCategory(newsCategoryId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{newsCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteNewsCategory(int newsCategoryId)
        {
            var res = _newsCategoryService.DeleteNewsCategory(newsCategoryId);
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
