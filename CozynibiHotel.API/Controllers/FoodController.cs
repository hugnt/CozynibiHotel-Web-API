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
    public class FoodController : Controller
    {
        private readonly IFoodService _foodService;
        private readonly IWebHostEnvironment _environment;

        public FoodController(IFoodService foodService, IWebHostEnvironment environment)
        {
            _foodService = foodService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Food>))]
        public IActionResult GetFoods()
        {
            var foods = _foodService.GetFoods();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(foods);
        }

        [HttpGet("{foodId}")]
        [ProducesResponseType(200, Type = typeof(Food))]
        [ProducesResponseType(400)]
        public IActionResult GetFood(int foodId)
        {
            var food = _foodService.GetFood(foodId);
            if (!ModelState.IsValid) return BadRequest();
            if (food == null) return NotFound();
            return Ok(food);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateFood([FromForm] FoodDto foodCreate, [FromForm] List<IFormFile> images)
        {
            if (foodCreate == null) return BadRequest(ModelState);

            var res = _foodService.CreateFood(foodCreate);

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
        [HttpPut("{foodId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateFood(int foodId, [FromForm] FoodDto updatedFood, [FromForm] List<IFormFile> images)
        {
            if (updatedFood == null) return BadRequest(ModelState);
            if (foodId != updatedFood.Id) return BadRequest(ModelState);

            var res = _foodService.UpdateFood(foodId, updatedFood);
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
        [HttpPut("{foodId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateFood(int foodId, bool isDelete)
        {

            var res = _foodService.UpdateFood(foodId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{foodId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFood(int foodId)
        {
            var res = _foodService.DeleteFood(foodId);
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<FoodDto>))]
        public IActionResult SearchFoodCategories(string field, string keyWords)
        {
            var foodCategories = _foodService.SearchFoods(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (foodCategories == null) return NotFound();
            return Ok(foodCategories);
        }


    }
}
