using CoreApi.ActionResult;
using DataAccess.Crud;
using EncryptPassword;
using Entities_POJO;
using Exceptions;
using System;
using System.Text;

namespace CoreApi
{
    public class ProfileManager : IProfileManager
    {
        private ProfileCrudFactory _crudFactory { get; set; }

        public ProfileManager()
        {
            _crudFactory = new ProfileCrudFactory();
        }

        public Profile GetProfile(Profile profile)
        {
            return _crudFactory.Retrieve<Profile>(profile);
        }

        public ManagerActionResult<Profile> RegisterProfile(ProfileForRegistration profileForRegistration)
        {
            try
            {
                var user = new User
                {
                    Username = profileForRegistration.Email,
                    Salt = Cryptographic.GenerateSalt()
                };
                user.Password = Cryptographic.HashPasswordWithSalt(Encoding.UTF8.GetBytes(profileForRegistration.Password), user.Salt);

                var newUser = new UserCrudFactory().Create<User>(user);

                if (newUser == null) return new ManagerActionResult<Profile>(null, ManagerActionStatus.Error);

                var profile = DataAccess.Factory.ProfileFactory.CreateProfile(profileForRegistration);
                profile.UserId = newUser.UserId;

                var newProfile = _crudFactory.Create<Profile>(profile);

                if (newProfile != null) return new ManagerActionResult<Profile>(newProfile, ManagerActionStatus.Created);

                return new ManagerActionResult<Profile>(null, ManagerActionStatus.NothingModified);
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                BussinessException exception;

                switch (sqlEx.Number)
                {
                    case 201:
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(2));//Missing parameters
                        break;
                    case 2627:
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(3));//Existing user
                        break;
                    default:
                        throw sqlEx;//Uncontrolled exception
                }
                return new ManagerActionResult<Profile>(null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ManagerActionResult<Profile> EditPicture(Profile profile)
        {
            try
            {
                int result = _crudFactory.EditPicture(profile);

                if (result != 0) return new ManagerActionResult<Profile>(profile, ManagerActionStatus.Created);

                return new ManagerActionResult<Profile>(profile, ManagerActionStatus.NothingModified);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface IProfileManager
    {
        ManagerActionResult<Profile> EditPicture(Profile profile);
        Profile GetProfile(Profile profile);
        ManagerActionResult<Profile> RegisterProfile(ProfileForRegistration profileForRegistration);
    }
}
