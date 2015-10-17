namespace Backend
{
    using Microsoft.AspNet.Identity;
    using System.Threading.Tasks;
    using System.Security.Claims;

    public class UserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<User>
    {
        public Task<ClaimsPrincipal> CreateAsync(User user)
        {
            return Task.Factory.StartNew( () => new ClaimsPrincipal(ClaimsPrincipal.Current.Identities));
        }
    }
}