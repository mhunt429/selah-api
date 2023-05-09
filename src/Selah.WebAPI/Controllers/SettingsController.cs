using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/settings")]
    public class SettingsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("health-check")]
        public ActionResult StatusCheck()
        {
            return Ok(new { statusCode = 200 });
        }
    }
}