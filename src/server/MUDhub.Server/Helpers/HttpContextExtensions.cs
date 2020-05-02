using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
