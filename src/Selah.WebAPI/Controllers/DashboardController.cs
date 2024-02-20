using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Selah.Application.Queries.Analytics;
using Selah.Domain.Data.Models.Analytics.Dashboard;
using Selah.Application.Filters;
using Selah.WebAPI.Extensions;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public DashboardController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummary>> GetDashboardSummary([FromRoute] UserDashboardQuery query)
        {
            query.UserId = Request.GetUserIdFromRequest();
            return Ok(await _mediatr.Send(query));
        }
    }
}