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
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IWebHostEnvironment _environment;

        public ArticleController(IArticleService articleService, IWebHostEnvironment environment)
        {
            _articleService = articleService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Article>))]
        public IActionResult GetArticles()
        {
            var articles = _articleService.GetArticles();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(articles);
        }

        [HttpGet("{articleId}")]
        [ProducesResponseType(200, Type = typeof(Article))]
        [ProducesResponseType(400)]
        public IActionResult GetArticle(int articleId)
        {
            var article = _articleService.GetArticle(articleId);
            if (!ModelState.IsValid) return BadRequest();
            if (article == null) return NotFound();
            return Ok(article);
        }

        [AllowAnonymous]
        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticleDto>))]
        public IActionResult SearchArticleCategories(string field, string keyWords)
        {
            var articleCategories = _articleService.SearchArticles(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (articleCategories == null) return NotFound();
            return Ok(articleCategories);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleDto articleCreate)
        {
            if (articleCreate == null) return BadRequest(ModelState);

            var res = _articleService.CreateArticle(articleCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [Authorize]
        [HttpPut("{articleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateArticle(int articleId, [FromForm] ArticleDto updatedArticle)
        {
            if (updatedArticle == null) return BadRequest(ModelState);
            if (articleId != updatedArticle.Id) return BadRequest(ModelState);

            var res = _articleService.UpdateArticle(articleId, updatedArticle);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{articleId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateArticle(int articleId, bool isDelete)
        {

            var res = _articleService.UpdateArticle(articleId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{articleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteArticle(int articleId)
        {
            var res = _articleService.DeleteArticle(articleId);
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
