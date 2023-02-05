using IdentityModel;
using IdentityServer.IdentityApi_AuthServer.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace IdentityServer.IdentityApiAuthServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManger;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManger)
        {
            _userManger = userManger;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {

            var exitUser = await _userManger.FindByEmailAsync(context.UserName);
            if (exitUser == null)
            {
                //loglama
                return;
            }
            var passwordCheck = await _userManger.CheckPasswordAsync(exitUser, context.Password);
            if (passwordCheck==false)
            {
                //loglama
                return;
            }

            context.Result = new GrantValidationResult(exitUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
