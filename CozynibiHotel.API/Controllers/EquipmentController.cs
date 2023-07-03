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
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _equipmentService;
        
        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EquipmentDto>))]
        public IActionResult GetEquipments()
        {
            var equipments = _equipmentService.GetEquipments();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(equipments);
        }

        [HttpGet("{equipmentId}")]
        [ProducesResponseType(200, Type = typeof(Equipment))]
        [ProducesResponseType(400)]
        public IActionResult GetEquipment(int equipmentId)
        {
            var equipment = _equipmentService.GetEquipment(equipmentId);
            if (!ModelState.IsValid) return BadRequest();
            if (equipment == null) return NotFound();
            return Ok(equipment);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateEquipment([FromBody] EquipmentDto equipmentCreate)
        {
            if (equipmentCreate == null) return BadRequest(ModelState);

            var res = _equipmentService.CreateEquipment(equipmentCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [HttpPut("{equipmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateEquipment(int equipmentId, [FromBody] EquipmentDto updatedEquipment)
        {
            if (updatedEquipment == null) return BadRequest(ModelState);
            if (equipmentId != updatedEquipment.Id) return BadRequest(ModelState);

            var res = _equipmentService.UpdateEquipment(equipmentId, updatedEquipment);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [HttpDelete("{equipmentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteEquipment(int equipmentId)
        {
            var res = _equipmentService.DeleteEquipment(equipmentId);
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
