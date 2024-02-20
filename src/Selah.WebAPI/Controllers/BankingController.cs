using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Filters;
using Selah.Application.Queries.Banking;
using Selah.WebAPI.Extensions;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/banking")]
    public class BankingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts([FromQuery(Name = "limit")] int limit,
            [FromQuery(Name = "offset")] int offset)
        {
            var query = new GetAllBankAccountsQuery
            {
                UserId = Request.GetUserIdFromRequest(),
                Limit = limit,
                Offset = offset
            };
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