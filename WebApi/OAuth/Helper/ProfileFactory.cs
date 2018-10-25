using Entities_POJO;
using OAuth.ViewModel;

namespace OAuth.Helper
{
    public class ProfileFactory
    {
        public static ProfileForRegistration GetProfileForRegistration(ViewModelForUserRegistration model)
        {
            return new ProfileForRegistration
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Identification = model.Identification,
            };
        }
    }
}