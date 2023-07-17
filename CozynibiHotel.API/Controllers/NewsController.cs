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
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _environment;

        public NewsController(INewsService newsService, IWebHostEnvironment environment)
        {
            _newsService = newsService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<News>))]
        public IActionResult GetNews()
        {
            var news = _newsService.GetNews();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(news);
        }

        [HttpGet("{newsId}")]
        [ProducesResponseType(200, Type = typeof(News))]
        [ProducesResponseType(400)]
        public IActionResult GetNews(int newsId)
        {
            var news = _newsService.GetNews(newsId);
            if (!ModelState.IsValid) return BadRequest();
            if (news == null) return NotFound();
            return Ok(news);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateNews([FromForm] NewsDto newsCreate, [FromForm] List<IFormFile> images)
        {
            if (newsCreate == null) return BadRequest(ModelState);

            var res = _newsService.CreateNews(newsCreate);

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
        [HttpPut("{newsId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateNews(int newsId, [FromForm] NewsDto updatedNews, [FromForm] List<IFormFile> images)
        {
            if (updatedNews == null) return BadRequest(ModelState);
            if (newsId != updatedNews.Id) return BadRequest(ModelState);

            var res = _newsService.UpdateNews(newsId, updatedNews);
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
        [HttpPut("{newsId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateNews(int newsId, bool isDelete)
        {

            var res = _newsService.UpdateNews(newsId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{newsId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteNews(int newsId)
        {
            var res = _newsService.DeleteNews(newsId);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }


        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsDto>))]
        public IActionResult SearchNews(string field, string keyWords)
        {
            var newsCategories = _newsService.SearchNews(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (newsCategories == null) return NotFound();
            return Ok(newsCategories);
        }


    }
}
