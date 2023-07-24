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
    public class GalleryController : Controller
    {
        private readonly IGalleryService _galleryService;
        private readonly IWebHostEnvironment _environment;

        public GalleryController(IGalleryService galleryService, IWebHostEnvironment environment)
        {
            _galleryService = galleryService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Gallery>))]
        public IActionResult GetGalleries()
        {
            var galleries = _galleryService.GetGalleries();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(galleries);
        }

        [HttpGet("{galleryId}")]
        [ProducesResponseType(200, Type = typeof(Gallery))]
        [ProducesResponseType(400)]
        public IActionResult GetGallery(int galleryId)
        {
            var gallery = _galleryService.GetGallery(galleryId);
            if (!ModelState.IsValid) return BadRequest();
            if (gallery == null) return NotFound();
            return Ok(gallery);
        }

        //Cate
        [HttpGet("GalleryCategory")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Gallery>))]
        public IActionResult GetGalleryCategory()
        {
            var galleries = _galleryService.GetGalleryCategories();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(galleries);
        }

        [HttpGet("GalleryCategory/{galleryCategoryId}")]
        [ProducesResponseType(200, Type = typeof(Gallery))]
        [ProducesResponseType(400)]
        public IActionResult GetGalleryCategory(int galleryCategoryId)
        {
            var gallery = _galleryService.GetGalleryCategory(galleryCategoryId);
            if (!ModelState.IsValid) return BadRequest();
            if (gallery == null) return NotFound();
            return Ok(gallery);
        }


        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGallery([FromForm] GalleryDto galleryCreate, [FromForm] List<IFormFile> images)
        {
            if (galleryCreate == null) return BadRequest(ModelState);

            var res = _galleryService.CreateGallery(galleryCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\gallery";
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
        [HttpPut("{galleryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateGallery(int galleryId, [FromForm] GalleryDto updatedGallery, [FromForm] List<IFormFile> images)
        {
            if (updatedGallery == null) return BadRequest(ModelState);
            if (galleryId != updatedGallery.Id) return BadRequest(ModelState);

            var res = _galleryService.UpdateGallery(galleryId, updatedGallery);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\gallery";
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
        [HttpPut("{galleryId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateGallery(int galleryId, bool isDelete)
        {

            var res = _galleryService.UpdateGallery(galleryId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{galleryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteGallery(int galleryId)
        {
            var res = _galleryService.DeleteGallery(galleryId);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GalleryDto>))]
        public IActionResult SearchGalleryCategories(string field, string keyWords)
        {
            var galleryCategories = _galleryService.SearchGalleries(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (galleryCategories == null) return NotFound();
            return Ok(galleryCategories);
        }


    }
}
