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
    public class FoodCategoryController : Controller
    {
        private readonly IFoodCategoryService _foodCategoryService;
        private readonly IWebHostEnvironment _environment;

        public FoodCategoryController(IFoodCategoryService foodCategoryService, IWebHostEnvironment environment)
        {
            _foodCategoryService = foodCategoryService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FoodCategoryDto>))]
        public IActionResult GetFoodCategories()
        {
            var foodCategories = _foodCategoryService.GetFoodCategories();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(foodCategories);
        }

        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FoodCategoryDto>))]
        public IActionResult SearchFoodCategories(string field, string keyWords)
        {
            var foodCategories = _foodCategoryService.SearchFoodCategories(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (foodCategories == null) return NotFound();
            return Ok(foodCategories);
        }

        [HttpGet("{foodCategoryId}")]
        [ProducesResponseType(200, Type = typeof(FoodCategory))]
        [ProducesResponseType(400)]
        public IActionResult GetFoodCategory(int foodCategoryId)
        {
            var foodCategory = _foodCategoryService.GetFoodCategory(foodCategoryId);
            if (!ModelState.IsValid) return BadRequest();
            if (foodCategory == null) return NotFound();
            return Ok(foodCategory);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateFoodCategory([FromForm] FoodCategoryDto foodCategoryCreate, [FromForm] List<IFormFile> images)
        {
            if (foodCategoryCreate == null) return BadRequest(ModelState);

            var res = _foodCategoryService.CreateFoodCategory(foodCategoryCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\menu";
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
        [HttpPut("{foodCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateFoodCategory(int foodCategoryId, [FromForm] FoodCategoryDto updatedFoodCategory, [FromForm] List<IFormFile> images)
        {
            if (updatedFoodCategory == null) return BadRequest(ModelState);
            if (foodCategoryId != updatedFoodCategory.Id) return BadRequest(ModelState);

            var res = _foodCategoryService.UpdateFoodCategory(foodCategoryId, updatedFoodCategory);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\menu";
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
        [HttpPut("{foodCategoryId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateFoodCategory(int foodCategoryId, bool isDelete)
        {

            var res = _foodCategoryService.UpdateFoodCategory(foodCategoryId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{foodCategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFoodCategory(int foodCategoryId)
        {
            var res = _foodCategoryService.DeleteFoodCategory(foodCategoryId);
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
