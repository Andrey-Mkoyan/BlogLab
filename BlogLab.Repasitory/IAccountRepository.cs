using Microsoft.AspNetCore.Identity;
using BlogLab.Models.Account;
namespace BlogLab.Repasitory
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> CreateAsync(ApplicationUserIdentity user,
            CancellationToken cancellationToken);
        public Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername,
            CancellationToken cancellationToken);

    }
}
