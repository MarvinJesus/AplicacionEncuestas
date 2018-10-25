using CoreApi;
using Entities_POJO;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuth.Config
{
    public class SurveyOnlineUserServices : UserServiceBase
    {
        private IProfileManager ProfileManager { get; set; }
        private IUserClaimManager UserClaimManager { get; set; }

        public SurveyOnlineUserServices(IProfileManager profileManager, IUserClaimManager userClaimManager)
        {
            this.ProfileManager = profileManager;
            this.UserClaimManager = userClaimManager;
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            Profile profile = await Task.Run(() =>
            {
                return ProfileManager.GetProfile(context.UserName, context.Password);
            });

            if (profile == null)
            {
                context.AuthenticateResult = new AuthenticateResult("Invalid credentials");

            }

            context.AuthenticateResult = new AuthenticateResult(profile.UserId.ToString(), context.UserName);
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();

            var profile = ProfileManager.GetProfile(new Profile { UserId = new Guid(userId) });

            if (profile != null)
                context.IssuedClaims = GetUserClaims(profile, context.RequestedClaimTypes);

            return Task.FromResult(0);
        }

        public IEnumerable<Claim> GetUserClaims(Profile profile, IEnumerable<string> requestedClaims)
        {
            ICollection<UserClaim> userClaims = UserClaimManager.GetUserClaims(profile);

            if (!(userClaims.Count > 0)) return null;

            return ConvertToClaims(userClaims.Where(uc => requestedClaims.Contains(uc.Type)));
        }

        public IEnumerable<Claim> ConvertToClaims(IEnumerable<UserClaim> userClaims)
        {
            if (userClaims == null) return null;

            List<Claim> claims = new List<Claim>();

            foreach (var userClaim in userClaims)
            {
                claims.Add(new Claim(userClaim.Type, userClaim.Value));
            }

            return claims;
        }
    }
}
