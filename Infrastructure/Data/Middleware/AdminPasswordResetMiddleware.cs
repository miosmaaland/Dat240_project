using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Infrastructure.Data;
using System;

namespace SmaHauJenHoaVij.Infrastructure.Middleware
{
    public class AdminPasswordResetMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminPasswordResetMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ShopContext dbContext)
        {
            // Check if the user is an admin
            var userType = context.Session.GetString("UserType");
            if (userType == "Administrator")
            {
                var userId = context.Session.GetString("UserId");
                if (!string.IsNullOrEmpty(userId))
                {
                    var adminId = Guid.Parse(userId);
                    var admin = await dbContext.Admins.FindAsync(adminId);

                    // Redirect if PasswordNeedsReset is true and the current request is not the ChangePassword page
                    if (admin != null && admin.PasswordNeedsReset &&
                        !context.Request.Path.StartsWithSegments("/AdminPages/ChangePassword", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Response.Redirect("/AdminPages/ChangePassword");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
