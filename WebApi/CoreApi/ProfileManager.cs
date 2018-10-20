using CoreApi.ActionResult;
using DataAccess.Crud;
using Dtos;
using Entities_POJO;
using Exceptions;

namespace CoreApi
{
    public class ProfileManager : IProfileManager
    {
        private ProfileCrudFactory _crudFactory { get; set; }

        public ProfileManager()
        {
            _crudFactory = new ProfileCrudFactory();
        }

        public Profile GetProfile(Profile Profile)
        {
            return _crudFactory.Retrieve<Profile>(Profile);
        }

        public ManagerActionResult<Profile> RegisterProfile(ProfileDto ProfileDto)
        {
            try
            {
                //if (ProfileDto.Password == null)
                //    throw new BussinessException(2);

                Profile Profile = DataAccess.Factories.ProfileFactory.CreateProfile(ProfileDto);

                var newUser = _crudFactory.Create<Profile>(Profile);

                if (newUser != null)
                {
                    return new ManagerActionResult<Profile>(newUser, ManagerActionStatus.Created);
                }
                else
                {
                    return new ManagerActionResult<Profile>(newUser, ManagerActionStatus.NothingModified, null);
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                BussinessException exception;

                switch (sqlEx.Number)
                {
                    case 201:
                        //Missing parameters
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(2));
                        break;
                    case 2627:
                        //Existing user
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(3));
                        break;
                    default:
                        //Uncontrolled exception
                        exception = ExceptionManager.GetInstance().Process(sqlEx);
                        break;
                }

                return new ManagerActionResult<Profile>(
                    null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Profile>(null, ManagerActionStatus.Error, exception);
            }
        }
    }

    public interface IProfileManager
    {
        Profile GetProfile(Profile Profile);
        ManagerActionResult<Profile> RegisterProfile(ProfileDto Profile);
    }
}
