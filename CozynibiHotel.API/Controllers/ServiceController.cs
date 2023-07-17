using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HUG.CRUD.Services;

namespace CozynibiHotel.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly IWebHostEnvironment _environment;

        public ServiceController(IServiceService serviceService, IWebHostEnvironment environment)
        {
            _serviceService = serviceService;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Service>))]
        public IActionResult GetServices()
        {
            var services = _serviceService.GetServices();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(services);
        }

        [HttpGet("{serviceId}")]
        [ProducesResponseType(200, Type = typeof(Service))]
        [ProducesResponseType(400)]
        public IActionResult GetService(int serviceId)
        {
            var service = _serviceService.GetService(serviceId);
            if (!ModelState.IsValid) return BadRequest();
            if (service == null) return NotFound();
            return Ok(service);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateService([FromForm] ServiceDto serviceCreate, [FromForm] List<IFormFile> images)
        {
            if (serviceCreate == null) return BadRequest(ModelState);

            var res = _serviceService.CreateService(serviceCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\service";
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
        [HttpPut("{serviceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateService(int serviceId, [FromForm] ServiceDto updatedService, [FromForm] List<IFormFile> images)
        {
            if (updatedService == null) return BadRequest(ModelState);
            if (serviceId != updatedService.Id) return BadRequest(ModelState);

            var res = _serviceService.UpdateService(serviceId, updatedService);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            var folderImage = "images\\service";
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
        [HttpPut("{serviceId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateService(int serviceId, bool isDelete)
        {

            var res = _serviceService.UpdateService(serviceId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }


        [Authorize]
        [HttpDelete("{serviceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteService(int serviceId)
        {
            var res = _serviceService.DeleteService(serviceId);
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceDto>))]
        public IActionResult SearchServiceCategories(string field, string keyWords)
        {
            var serviceCategories = _serviceService.SearchServices(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (serviceCategories == null) return NotFound();
            return Ok(serviceCategories);
        }


    }
}
