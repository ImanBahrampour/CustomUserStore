using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Project.Authorization.Roles;
using Project.Authorization.Users;
using Project.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Configuration
{
    public class CustomUserStore : AbpUserStore<Role, User> 
    {
        private  CustomSession _customSession { get; set; }
        private readonly IRepository<Role> _roleRepository;
        private  IRepository<UserRole, long> _userRoleRepository { get; set; }
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CustomUserStore(CustomSession customSession,IUnitOfWorkManager unitOfWorkManager, IRepository<User, long> userRepository,
            IRepository<Role> roleRepository, IRepository<UserRole, long> userRoleRepository,
            IRepository<UserLogin, long> userLoginRepository, IRepository<UserClaim, long> userClaimRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
            : base(unitOfWorkManager, userRepository, roleRepository, userRoleRepository,
                  userLoginRepository, userClaimRepository, userPermissionSettingRepository,
                  userOrganizationUnitRepository, organizationUnitRoleRepository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _customSession = customSession;
        }
        public async override Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
        {
            return await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                Check.NotNull(user, nameof(user));

                var userRoles = await AsyncQueryableExecuter.ToListAsync(from userRole in _userRoleRepository.GetAll()
                                                                         join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                                                                         where userRole.UserId == user.Id
                                                                         select role.Name);
                //get organization Ids from session
                long[] oUIds = _customSession.OUIds.Split("-").Select(long.Parse).ToArray();

                var userOrganizationUnitRoles = await AsyncQueryableExecuter.ToListAsync(
                    from userOu in _userOrganizationUnitRepository.GetAll().Where(o=> oUIds.Contains(o.Id))
                    join roleOu in _organizationUnitRoleRepository.GetAll() on userOu.OrganizationUnitId equals roleOu
                        .OrganizationUnitId
                    join userOuRoles in _roleRepository.GetAll() on roleOu.RoleId equals userOuRoles.Id
                    where userOu.UserId == user.Id
                    select userOuRoles.Name);

                return userRoles.Union(userOrganizationUnitRoles).ToList();
            });
        }
    }
}
