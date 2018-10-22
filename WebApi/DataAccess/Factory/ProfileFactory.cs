using Entities_POJO;

namespace DataAccess.Factory
{
    public class ProfileFactory
    {
        public static Profile CreateProfile(ProfileForRegistration profile)
        {
            var Profile = new Profile
            {
                Name = profile.Name,
                Identification = profile.Identification,
                Email = profile.Email,
                ImagePath = "defaultProfilePicture.png"
            };

            return Profile;
        }
    }
}
