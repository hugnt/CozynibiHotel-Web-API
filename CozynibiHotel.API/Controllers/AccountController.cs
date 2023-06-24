using CozynibiHotel.Core.Models;
using CozynibiHotel.Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CozynibiHotel.Services.Interfaces;

namespace CozynibiHotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Account>))]
        public IActionResult GetAccounts()
        {
            var accounts = _accountService.GetAccounts();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(accounts);
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(200, Type = typeof(Account))]
        [ProducesResponseType(400)]
        public IActionResult GetAccount(int accountId)
        {
            var account = _accountService.GetAccount(accountId);
            if (!ModelState.IsValid) return BadRequest();
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateAccount([FromBody] AccountDto accountCreate)
        {
            if (accountCreate == null) return BadRequest(ModelState);

            var res = _accountService.CreateAccount(accountCreate);

            if (res.Status != 201)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return StatusCode(res.Status, res.StatusMessage);
        }

        [HttpPut("{accountId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAccount(int accountId, [FromBody] AccountDto updatedAccount)
        {
            if (updatedAccount == null) return BadRequest(ModelState);
            if (accountId != updatedAccount.Id) return BadRequest(ModelState);

            var res = _accountService.UpdateAccount(accountId, updatedAccount);
            if (res.Status != 204)
            {
                ModelState.AddModelError("", res.StatusMessage);
                return StatusCode(res.Status, ModelState);
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();
        }

        [HttpDelete("{accountId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAccount(int accountId)
        {
            var res = _accountService.DeleteAccount(accountId);
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
