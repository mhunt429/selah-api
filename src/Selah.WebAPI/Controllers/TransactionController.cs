using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Commands.Transactions;
using Selah.Application.Queries;
using Selah.WebAPI.Extensions;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/transactions")]
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
            command.Data.UserId = Request.GetUserIdFromRequest();
            var result = await _mediator.Send(command);
            if (result.IsLeft)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("categories/totals")]
        public async Task<IActionResult> GetTransactionTotalsByCategory(
            [FromRoute] TransactionTotalsByCategoryQuery query)
        {
            query.UserId = Request.GetUserIdFromRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}