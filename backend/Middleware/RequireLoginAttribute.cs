namespace AirbnbREST.Middleware;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// protects API routes
// checks if userId exists in the session (user logged in)
// If not logged in, returns 401 Unauthorized
// If logged in, allows request to proceed
public class RequireLoginAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedObjectResult("You must be logged in to access this resource.");
        }
    }
}
