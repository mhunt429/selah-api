using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Selah.Domain.Data.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

namespace Selah.WebAPI.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserIdFromRequest(this HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out StringValues authToken))
            {
                var jwt = authToken.ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                return token.Claims.First().Value;
            }

            return "";
        }

        public static IEnumerable<ValidationError> GetValidationErrors(this ValidationResult validationResult)
        {
            return validationResult.Errors.Select(x => new ValidationError
            {
                PropertyName = x.PropertyName,
                ErrorMessage = x.ErrorMessage,
                AttemtedValue = x.AttemptedValue?.ToString()
            });
        }

        public static string GetIpAddressFromRequest(this HttpRequest request)
        {
            return request.HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        public static string GetRequestTraceId(this HttpRequest request)
        {
            return request.HttpContext.TraceIdentifier;
        }

        public static IActionResult MapEitherToHttpResult<L, R>(this Either<L, R> result, ControllerBase controller)
            where L : ValidationResult
        {
            IActionResult ToActionResultLeft(L error) => controller.BadRequest(error.GetValidationErrors());
            IActionResult ToActionResultRight(R value) => controller.Ok(value);

            return result.Match(
                Left: ToActionResultLeft,
                Right: ToActionResultRight
            );
        }
    }
}