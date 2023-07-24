using AutoMapper;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CozynibiHotel.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class InformationController : Controller
    {
        private readonly IInformationService _informationService;

        public InformationController(IInformationService informationService)
        {
            _informationService = informationService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Information>))]
        public IActionResult GetInformation()
        {
            var informations = _informationService.GetInformation();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(informations);
        }

        [Authorize]
        [HttpPut("Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateInformation([FromBody] InformationDto updatedInformation)
        {
            if (updatedInformation == null) return BadRequest(ModelState);

            var res = _informationService.UpdateInformation(updatedInformation);
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
