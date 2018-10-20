using Dtos;
using Entities_POJO;

namespace DataAccess.Factories
{
    public class ProfileFactory
    {
        public static Profile CreateProfile(ProfileDto ProfileDto)
        {
            var Profile = new Profile
            {
                Id = ProfileDto.Id,
                Name = ProfileDto.Name,
                Identification = ProfileDto.Identification,
                Email = ProfileDto.Email,
                ImagePath = ProfileDto.ImagePath
            };

            //Profile.Password = Cryptographic.HashPasswordWithSalt(Encoding.UTF8.GetBytes(ProfileDto.Password), Profile.Salt);

            return Profile;
        }

        public static ProfileDto CreateProfile(Profile Profile)
        {
            return new ProfileDto
            {
                Id = Profile.Id,
                Name = Profile.Name,
                Identification = Profile.Identification,
                Email = Profile.Email,
                ImagePath = Profile.ImagePath
            };
        }
    }
}
