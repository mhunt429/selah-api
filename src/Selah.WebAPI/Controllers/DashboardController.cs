using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Selah.Domain.Data.Models.Investments;
using Microsoft.AspNetCore.Authorization;
using Selah.Domain.Data.Models.Integrations;
using Selah.Domain.Data.Models.Integrations.TDAmeritrade;
using Selah.Application.Services.Interfaces;
using MediatR;
using Selah.Application.Queries.Analytics;
using Selah.Domain.Data.Models.Analytics.Dashboard;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/users/{userId}/dashboard")]
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
            return Ok(await _mediatr.Send(query));
        }
    }
}
