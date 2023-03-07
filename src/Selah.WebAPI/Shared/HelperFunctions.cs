using Microsoft.AspNetCore.Http;
using Selah.Domain.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Selah.WebAPI.Shared
{
    public class HelperFunctions
    {
        public static string ParseExceptionAsString(Exception ex)
        {
            return $"\nError: {ex.Source}\nMessage: {ex.Message}\nStack: {ex.StackTrace}";
        }

        public static string GetIpAddressFromRequest(HttpRequest request)
        {
            return request.HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        public static string GetRequestTraceId(HttpRequest request)
        {
            return request.HttpContext.TraceIdentifier;
        }
      
    }
}