using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Services.Interfaces;

namespace Selah.WebAPI.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/v1/settings")]
  public class SettingsController: ControllerBase
  {
    private readonly IPlaidService _plaidService;

    public SettingsController(IPlaidService plaidService)
    {
      _plaidService = plaidService;
    }

    [HttpGet("plaid-public-token")]
    public async Task<ActionResult> GetPlaidPublicToken()
    {
      var publicToken = await _plaidService.GetLinkToken();
      if (publicToken == null)
      {
        return BadRequest();
      }

      return Ok(publicToken);
    }

    [AllowAnonymous]
    [HttpGet("health-check")]
    public ActionResult StatusCheck()
    {
      return Ok(new { statusCode = 200 });
    }
  }
  
}