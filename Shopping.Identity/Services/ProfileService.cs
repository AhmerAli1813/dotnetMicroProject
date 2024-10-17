using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Shopping.Identity.Models;
using System.Security.Claims;

namespace Shopping.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaims;

        public ProfileService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> userRole, IUserClaimsPrincipalFactory<ApplicationUser> userClaims)
        {
            _userManager = userManager;
            _roleManager = userRole;
            _userClaims = userClaims;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(subjectId);
            ClaimsPrincipal claims = await _userClaims.CreateAsync(user);
            List<Claim> claimsList = claims.Claims.ToList();
            claimsList = claimsList.Where(c => context.RequestedClaimTypes.Contains(c.Type)).ToList();
            claimsList.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            claimsList.Add(new Claim(JwtClaimTypes.Email, user.Email));
            if (_userManager.SupportsUserRole) {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                foreach (string roleName in roles) {
                    claimsList.Add(new Claim(JwtClaimTypes.Role, roleName));
                    if (_roleManager.SupportsRoleClaims) { 
                        IdentityRole Roles  = await _roleManager.FindByNameAsync(roleName);
                        if (Roles != null) {
                            claimsList.AddRange(await _roleManager.GetClaimsAsync(Roles));
                         }
                    }
                }
            }
            context.IssuedClaims = claimsList;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string subjectId = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(subjectId);
            context.IsActive  = user!= null;

        }
    }
}
