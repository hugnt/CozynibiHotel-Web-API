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
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IWebHostEnvironment _environment;

        public PageController(IPageService pageService, IWebHostEnvironment environment)
        {
            _pageService = pageService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Page>))]
        public IActionResult GetPages()
        {
            var roomCategories = _pageService.GetPages();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(roomCategories);
        }

        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PageDto>))]
        public IActionResult SearchPages(string field, string keyWords)
        {
            var roomCategories = _pageService.SearchPages(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (roomCategories == null) return NotFound();
            return Ok(roomCategories);
        }

        [HttpGet("{pageId}")]
        [ProducesResponseType(200, Type = typeof(Page))]
        [ProducesResponseType(400)]
        public IActionResult GetPage(int pageId)
        {
            var page = _pageService.GetPage(pageId);
            if (!ModelState.IsValid) return BadRequest();
            if (page == null) return NotFound();
            return Ok(page);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePage([FromForm] PageDto pageCreate, [FromForm] List<IFormFile> images)
        {
            if (pageCreate == null) return BadRequest(ModelState);

            var res = _pageService.CreatePage(pageCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\banner";
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
        [HttpPut("{pageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePage(int pageId, [FromForm] PageDto updatedPage, [FromForm] List<IFormFile> images)
        {
            if (updatedPage == null) return BadRequest(ModelState);
            if (pageId != updatedPage.Id) return BadRequest(ModelState);

            var res = _pageService.UpdatePage(pageId, updatedPage);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\banner";
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
        [HttpPut("{pageId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePage(int pageId, bool isDelete)
        {

            var res = _pageService.UpdatePage(pageId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{pageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePage(int pageId)
        {
            var res = _pageService.DeletePage(pageId);
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
