using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Project.Authorization.Roles;
using Abp.Domain.Uow;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace Project.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        public UserManager _userManager { get; set; }

        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor,
                  unitOfWorkManager)
        {
            this._userManager = userManager;
        }
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {

            var claim = await base.CreateAsync(user);
            claim.Identities.First().AddClaim(new Claim("Name", user.Name));
            claim.Identities.First().AddClaim(new Claim("UserName", user.UserName));
            var result = _userManager.GetOrganizationUnits(user).Select(o => o.Id);
            claim.Identities.First().AddClaim(new Claim("OUIds", string.Join('-', _userManager.GetOrganizationUnits(user).Select(o => o.Id))));
            return claim;
        }
    }
}
