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
    public class LanguageController : Controller
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Language>))]
        public IActionResult GetLanguages()
        {
            var languages = _languageService.GetLanguages();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(languages);
        }

        [HttpGet("{languageId}")]
        [ProducesResponseType(200, Type = typeof(Language))]
        [ProducesResponseType(400)]
        public IActionResult GetLanguage(int languageId)
        {
            var language = _languageService.GetLanguage(languageId);
            if (!ModelState.IsValid) return BadRequest();
            if (language == null) return NotFound();
            return Ok(language);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateLanguage([FromBody] LanguageDto languageCreate)
        {
            if (languageCreate == null) return BadRequest(ModelState);

            var res = _languageService.CreateLanguage(languageCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [Authorize]
        [HttpPut("{languageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateLanguage(int languageId, [FromBody] LanguageDto updatedLanguage)
        {
            if (updatedLanguage == null) return BadRequest(ModelState);
            if (languageId != updatedLanguage.Id) return BadRequest(ModelState);

            var res = _languageService.UpdateLanguage(languageId, updatedLanguage);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{languageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteLanguage(int languageId)
        {
            var res = _languageService.DeleteLanguage(languageId);
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
