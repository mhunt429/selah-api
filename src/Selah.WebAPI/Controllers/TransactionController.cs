using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Commands.Transactions;
using Selah.Application.Filters;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [UserIdParamMatchesClaims]
    [Route("api/v1/{userId}/transactions")]
    public class TransactionController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}