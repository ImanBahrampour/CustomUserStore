using Abp.Authorization;
using Project.Authorization.Roles;
using Project.Authorization.Users;

namespace Project.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
