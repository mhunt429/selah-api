using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Commands.Transactions;
using Selah.Application.Filters;
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
        private readonly IValidator<CreateTransactionCommand> _validator;

        public TransactionController(IMediator mediator, IValidator<CreateTransactionCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            command.UserId = Request.GetUserIdFromRequest();
             var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetValidationErrors());
            }

            var result = await _mediator.Send(command);
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