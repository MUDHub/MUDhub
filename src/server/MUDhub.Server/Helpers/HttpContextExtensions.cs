using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace MUDhub.Server.Helpers
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            return context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        }
    }
}
