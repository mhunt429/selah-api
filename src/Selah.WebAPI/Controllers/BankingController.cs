using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Queries.Banking;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/users/{userId}/banking")]
    public class BankingController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BankingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts([FromRoute] Guid userId, [FromQuery(Name = "limit")] int limit, [FromQuery(Name = "offset")] int offset)
        {
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            var query = new GetAllBankAccountsQuery { UserId =  userId, Limit  = limit, Offset = offset };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("account")]
        public async Task CreateManualAccount()
        {

        }

        [HttpPut("account/{id}")]
        public async Task UpdateAccount()
        {

        }
    }
}