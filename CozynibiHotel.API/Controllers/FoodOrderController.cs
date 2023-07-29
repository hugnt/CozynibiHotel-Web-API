using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;
using HUG.CRUD.Services;
using Microsoft.AspNetCore.Authorization;
using CozynibiHotel.API.Hub;
using Microsoft.AspNetCore.SignalR;

namespace CozynibiHotel.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class FoodOrderController : Controller
    {
        private readonly IFoodOrderService _foodOrderService;
        private readonly IWebHostEnvironment _environment;
        private IHubContext<MessageHub, IMessageHubClient> _messageHub;

        public FoodOrderController(IFoodOrderService foodOrderService
                                  ,IWebHostEnvironment environment
                                  ,IHubContext<MessageHub, IMessageHubClient> messageHub)
        {
            _foodOrderService = foodOrderService;
            _environment = environment;
            _messageHub = messageHub;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FoodOrderDto>))]
        public IActionResult GetFoodOrders()
        {
            var foodCategories = _foodOrderService.GetFoodOrders();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(foodCategories);
        }
        [Authorize]
        [HttpGet("{field}/{keyWords}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FoodOrderDto>))]
        public IActionResult SearchFoodOrders(string field, string keyWords)
        {
            var foodCategories = _foodOrderService.SearchFoodOrders(field, keyWords);
            if (!ModelState.IsValid) return BadRequest();
            if (foodCategories == null) return NotFound();
            return Ok(foodCategories);
        }
        [Authorize]
        [HttpGet("{foodOrderId}")]
        [ProducesResponseType(200, Type = typeof(FoodOrder))]
        [ProducesResponseType(400)]
        public IActionResult GetFoodOrder(int foodOrderId)
        {
            var foodOrder = _foodOrderService.GetFoodOrder(foodOrderId);
            if (!ModelState.IsValid) return BadRequest();
            if (foodOrder == null) return NotFound();
            return Ok(foodOrder);
        }

        [AllowAnonymous]
        [HttpGet("CheckInCode/{checkInCode}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult TryCheckInCode(string checkInCode)
        {
            var checkInCode_int = int.Parse(checkInCode);
            if (!ModelState.IsValid) return BadRequest();
            var isValidCode = _foodOrderService.IsValidCheckInCode(checkInCode_int);
            if (!ModelState.IsValid) return BadRequest();
            if (isValidCode == false) return NotFound();
            return Ok(isValidCode);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateFoodOrder([FromForm] FoodOrderDto foodOrderCreate)
        {
            if (foodOrderCreate == null) return BadRequest(ModelState);

            var res = _foodOrderService.CreateFoodOrder(foodOrderCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            _messageHub.Clients.All.SendNotificationFoodOrder(foodOrderCreate);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [Authorize]
        [HttpPut("{foodOrderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateFoodOrder(int foodOrderId, [FromForm] FoodOrderDto updatedFoodOrder)
        {
            if (updatedFoodOrder == null) return BadRequest(ModelState);
            if (foodOrderId != updatedFoodOrder.Id) return BadRequest(ModelState);

            var res = _foodOrderService.UpdateFoodOrder(foodOrderId, updatedFoodOrder);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{foodOrderId}/{isDelete}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateFoodOrder(int foodOrderId, bool isDelete)
        {

            var res = _foodOrderService.UpdateFoodOrder(foodOrderId, isDelete);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{foodOrderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFoodOrder(int foodOrderId)
        {
            var res = _foodOrderService.DeleteFoodOrder(foodOrderId);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [Authorize]
        [HttpPut("{foodOrderId}/Status/{status}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateFoodOrderStatus(int foodOrderId, bool status)
        {

            var res = _foodOrderService.UpdateFoodOrderStatus(foodOrderId, status);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            return NoContent();
        }



    }
}
