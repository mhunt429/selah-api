using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Selah.WebAPI.Middleware
{
  public class LoggerMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    
    public LoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
      _next = next;
      _logger = loggerFactory.CreateLogger<LoggerMiddleware>();
    }
    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      finally
      {
        _logger.LogInformation(
           $"Remote IP Address: {context.Request.HttpContext.Connection.RemoteIpAddress?.ToString()} => " + "Request {method} {url} => {statusCode}",
          context.Request?.Method,
          context.Request?.Path.Value,
          context.Response?.StatusCode
          );
      }
    }
  }
}