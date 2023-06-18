using AutoMapper;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CozynibiHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : Controller
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IMapper _mapper;

        public LanguageController(ILanguageRepository languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Language>))]
        public IActionResult GetLanguages()
        {
            var languages = _mapper.Map<List<LanguageDto>>(_languageRepository.GetAll());
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(languages);
        }

        [HttpGet("{languageId}")]
        [ProducesResponseType(200, Type = typeof(Language))]
        [ProducesResponseType(400)]
        public IActionResult GetLanguage(int languageId)
        {
            if (!_languageRepository.IsExists(languageId)) return NotFound();

            var language = _mapper.Map<LanguageDto>(_languageRepository.GetById(languageId));
            if (!ModelState.IsValid) return BadRequest();

            return Ok(language);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateLanguage([FromBody] LanguageDto languageCreate)
        {
            if (languageCreate == null) return BadRequest(ModelState);

            var languages = _languageRepository.GetAll()
                            .Where(l => l.Name.Trim().ToLower() == languageCreate.Name.Trim().ToLower())
                            .FirstOrDefault();

            if (languages != null)
            {
                ModelState.AddModelError("", "Language already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var languageMap = _mapper.Map<Language>(languageCreate);

            if (!_languageRepository.Create(languageMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{languageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateLanguage(int languageId, [FromBody] LanguageDto updatedLanguage)
        {
            if (updatedLanguage == null) return BadRequest(ModelState);
            if (languageId != updatedLanguage.Id) return BadRequest(ModelState);
            if (!_languageRepository.IsExists(languageId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var languageMap = _mapper.Map<Language>(updatedLanguage);

            if (!_languageRepository.Update(languageMap))
            {
                ModelState.AddModelError("", "Something went wrong updating language");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{languageId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteLanguage(int languageId)
        {
            if (!_languageRepository.IsExists(languageId)) return NotFound();

            var languageToDelete = _languageRepository.GetById(languageId);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_languageRepository.Delete(languageToDelete))
            {
                ModelState.AddModelError("", "Something went wrong when deleting language");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



    }
}
