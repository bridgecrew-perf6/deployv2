using System.Security.Claims;

namespace backend.Recycle.Extensions
{
    public static class UserService
    {
        public static string GetUserId(this ClaimsPrincipal user) =>
            user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        public static string GetUserName(this ClaimsPrincipal user)=>
        user.Claims.FirstOrDefault(c=>c.Type == "Name")?.Value;

        public static string GetUserEmail(this ClaimsPrincipal user) =>
            user.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
    }
}
