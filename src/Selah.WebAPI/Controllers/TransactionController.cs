using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Commands.Transactions;
using Selah.Application.Services.Interfaces;
using Selah.Domain.Data.Models;
using Selah.Domain.Data.Models.ApplicationUser;
using Selah.Domain.Data.Models.Transactions;
using Selah.WebAPI.Extensions;
using Selah.WebAPI.Shared;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IAuthenticationService _authService;
        private readonly IMediator _mediator;

        public TransactionController(ITransactionService transactionService, IAuthenticationService authService, IMediator mediator)
        {
            _transactionService = transactionService;
            _authService = authService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery(Name = "take")] int take)
        {
            var userId = Request.GetUserIdFromRequest();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
           var transactions = await _transactionService.GetTransactions(userId, take);
            return Ok(transactions);
        }

        /*
        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactions([FromQuery(Name = "institutionId")] Guid? institutionId)
        {
            if (institutionId == null)
            {
                return BadRequest(new HttpResponseViewModel<AppUser>
                {
                    StatusCode = 400,

                });
            }
            try
            {
                await _transactionService.ImportTransactions(institutionId.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                var errors = new List<ErrorMessage>();
                errors.Add(new ErrorMessage { Key = null, Message = ex.ParseExceptionAsString() });
                return BadRequest(errors);
            }
        }*/

        //TODO add validation to this? Required data annotations should be enough

        [HttpPost("categories")]
        public async Task<IActionResult> CreateTransactionCategory([FromBody] UserTransactionCategoryCreate categoryCreate)
        {
            var userId = Request.GetUserIdFromRequest();
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            categoryCreate.UserId = userId;
            var category = await _transactionService.CreateTransactionCategory(categoryCreate);

            return Ok(category);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetTransactionCategoriesByUser()
        {

            var userId =
              _authService.GetUserFromClaims(Request);
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }

            var categories = await _transactionService.GetTransactionCategoriesByUser(userId);
            return Ok(categories);
        }

        [HttpGet("{transactionId}/summary")]
        public async Task<IActionResult> GetTransactionSummary(Guid transactionId)
        {
            var summary = await _transactionService.GetItemizedTransactionAsync(transactionId);
            return summary.Any() ? Ok(summary) : NotFound();
        }
    }
}