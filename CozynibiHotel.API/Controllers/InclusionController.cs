using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CozynibiHotel.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InclusionController : Controller
    {
        private readonly IInclusionService _inclusionService;
        
        public InclusionController(IInclusionService inclusionService)
        {
            _inclusionService = inclusionService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<InclusionDto>))]
        public IActionResult GetInclusions()
        {
            var inclusions = _inclusionService.GetInclusions();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(inclusions);
        }

        [HttpGet("{inclusionId}")]
        [ProducesResponseType(200, Type = typeof(Inclusion))]
        [ProducesResponseType(400)]
        public IActionResult GetInclusion(int inclusionId)
        {
            var inclusion = _inclusionService.GetInclusion(inclusionId);
            if (!ModelState.IsValid) return BadRequest();
            if (inclusion == null) return NotFound();
            return Ok(inclusion);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateInclusion([FromBody] InclusionDto inclusionCreate)
        {
            if (inclusionCreate == null) return BadRequest(ModelState);

            var res = _inclusionService.CreateInclusion(inclusionCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [HttpPut("{inclusionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateInclusion(int inclusionId, [FromBody] InclusionDto updatedInclusion)
        {
            if (updatedInclusion == null) return BadRequest(ModelState);
            if (inclusionId != updatedInclusion.Id) return BadRequest(ModelState);

            var res = _inclusionService.UpdateInclusion(inclusionId, updatedInclusion);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [HttpDelete("{inclusionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteInclusion(int inclusionId)
        {
            var res = _inclusionService.DeleteInclusion(inclusionId);
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
