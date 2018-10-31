using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Factory
{
    public class ProfileFactory : EntityFactory
    {
        private List<string> MAIN_PROPERTIES { get; set; }

        public ProfileFactory()
        {
            var profileDefault = new Profile();

            MAIN_PROPERTIES = new List<string>
            {
                nameof(profileDefault.UserId),
                nameof(profileDefault.Name),
                nameof(profileDefault.Identification),
                nameof(profileDefault.Email),
                nameof(profileDefault.ImagePath)
            };
        }

        public object CreateDataShapeObject(Profile topic)
        {
            return CreateDataShapeObject(topic, MAIN_PROPERTIES);
        }

        public object CreateDataShapeObject(Profile topic, List<string> listOfFields)
        {
            return CreateDataShapeObject(topic, listOfFields, null);
        }

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
