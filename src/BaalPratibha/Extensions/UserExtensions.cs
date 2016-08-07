using System.Security.Claims;
using ResumeMaker.Common.Extensions;

namespace BaalPratibha.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindClaim(ClaimTypes.Name);
            return claim?.Value;
        }

        public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindClaim(ClaimTypes.GivenName);
            return claim?.Value;
        }

        public static string GetSuburb(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindClaim(ClaimTypes.StateOrProvince);
            return claim?.Value;
        }

        private static Claim FindClaim(this ClaimsPrincipal claimsPrincipal, string httpSchemasXmlsoapOrgWsIdentityClaimsName)
        {
            return ((ClaimsIdentity)claimsPrincipal.Identity).FindFirst(httpSchemasXmlsoapOrgWsIdentityClaimsName);
        }

        public static long GetId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindClaim(ClaimTypes.Sid);
            return claim?.Value.ToLong() ?? 0;
        }
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindClaim(ClaimTypes.Email);
            return claim?.Value;
        }

        public static string GetRole(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindClaim(ClaimTypes.Role);
            return claim?.Value;
        }

        public static string GetPublicProfile(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindClaim("public_profile");
            return claim?.Value;
        }
    }
}
