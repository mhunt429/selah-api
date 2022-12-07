using System;

namespace Selah.WebAPI.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ParseExceptionAsString(this Exception ex)
        {
            return $"\nError: {ex.Source}\nMessage: {ex.Message}\nStack: {ex.StackTrace}";
        }
    }
}
