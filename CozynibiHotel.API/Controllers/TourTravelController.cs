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
    public class TourTravelController : Controller
    {
        private readonly ITourTravelService _tourTravelService;
        private readonly IWebHostEnvironment _environment;

        public TourTravelController(ITourTravelService tourTravelService, IWebHostEnvironment environment)
        {
            _tourTravelService = tourTravelService;
            _environment = environment;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TourTravelDto>))]
        public IActionResult GetTourTravels()
        {
            var foodCategories = _tourTravelService.GetTourTravels();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(foodCategories);
        }

        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TourTravelDto>))]
        public IActionResult SearchTourTravels(string field, string keyWords)
        {
            var foodCategories = _tourTravelService.SearchTourTravels(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (foodCategories == null) return NotFound();
            return Ok(foodCategories);
        }

        [HttpGet("{tourTravelId}")]
        [ProducesResponseType(200, Type = typeof(TourTravel))]
        [ProducesResponseType(400)]
        public IActionResult GetTourTravel(int tourTravelId)
        {
            var tourTravel = _tourTravelService.GetTourTravel(tourTravelId);
            if (!ModelState.IsValid) return BadRequest();
            if (tourTravel == null) return NotFound();
            return Ok(tourTravel);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTourTravel([FromForm] TourTravelDto tourTravelCreate, [FromForm] List<IFormFile> images)
        {
            if (tourTravelCreate == null) return BadRequest(ModelState);

            var res = _tourTravelService.CreateTourTravel(tourTravelCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\tour_travel_2";
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
        [HttpPut("{tourTravelId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTourTravel(int tourTravelId, [FromForm] TourTravelDto updatedTourTravel, [FromForm] List<IFormFile> images)
        {
            if (updatedTourTravel == null) return BadRequest(ModelState);
            if (tourTravelId != updatedTourTravel.Id) return BadRequest(ModelState);

            var res = _tourTravelService.UpdateTourTravel(tourTravelId, updatedTourTravel);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\tour_travel_2";
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
        [HttpPut("{tourTravelId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTourTravel(int tourTravelId, bool isDelete)
        {

            var res = _tourTravelService.UpdateTourTravel(tourTravelId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{tourTravelId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTourTravel(int tourTravelId)
        {
            var res = _tourTravelService.DeleteTourTravel(tourTravelId);
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
