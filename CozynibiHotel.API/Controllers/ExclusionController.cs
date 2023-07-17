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
    public class ExclusionController : Controller
    {
        private readonly IExclusionService _exclusionService;
        
        public ExclusionController(IExclusionService exclusionService)
        {
            _exclusionService = exclusionService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ExclusionDto>))]
        public IActionResult GetExclusions()
        {
            var exclusions = _exclusionService.GetExclusions();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(exclusions);
        }

        [HttpGet("{exclusionId}")]
        [ProducesResponseType(200, Type = typeof(Exclusion))]
        [ProducesResponseType(400)]
        public IActionResult GetExclusion(int exclusionId)
        {
            var exclusion = _exclusionService.GetExclusion(exclusionId);
            if (!ModelState.IsValid) return BadRequest();
            if (exclusion == null) return NotFound();
            return Ok(exclusion);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateExclusion([FromBody] ExclusionDto exclusionCreate)
        {
            if (exclusionCreate == null) return BadRequest(ModelState);

            var res = _exclusionService.CreateExclusion(exclusionCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [HttpPut("{exclusionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateExclusion(int exclusionId, [FromBody] ExclusionDto updatedExclusion)
        {
            if (updatedExclusion == null) return BadRequest(ModelState);
            if (exclusionId != updatedExclusion.Id) return BadRequest(ModelState);

            var res = _exclusionService.UpdateExclusion(exclusionId, updatedExclusion);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [HttpDelete("{exclusionId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteExclusion(int exclusionId)
        {
            var res = _exclusionService.DeleteExclusion(exclusionId);
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
