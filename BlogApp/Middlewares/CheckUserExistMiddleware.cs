using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace BlogApp.Middlewares;

public class CheckUserExistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IUserRepository _userRepository;

    public CheckUserExistMiddleware(RequestDelegate next, IUserRepository userRepository)
    {
        _next = next;
        _userRepository = userRepository;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            var email = context.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Email);

            if (email != null && !string.IsNullOrWhiteSpace(email.Value)) {
                var user = await _userRepository.GetByEmailAsync(email.Value);

                
            }

            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _next(context);
            return;
        }

        await _next(context);
    }
}
