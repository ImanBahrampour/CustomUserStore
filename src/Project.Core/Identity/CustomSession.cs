using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Identity
{
    public class CustomSession : ClaimsAbpSession, ITransientDependency
    {
        public CustomSession(
            IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) :
            base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {

        }

        public string UserName
        {
            get
            {
                var userNameClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "UserName");
                if (string.IsNullOrEmpty(userNameClaim?.Value))
                {
                    return null;
                }

                return userNameClaim.Value;
            }
        }
        public string Name
        {
            get
            {
                var NameClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "Name");
                if (string.IsNullOrEmpty(NameClaim?.Value))
                {
                    return null;
                }

                return NameClaim.Value;
            }
        }
        public string OUIds
        {
            get
            {
                var NameClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "OUIds");
                if (string.IsNullOrEmpty(NameClaim?.Value))
                {
                    return null;
                }

                return NameClaim.Value;
            }
        }
    }
}
