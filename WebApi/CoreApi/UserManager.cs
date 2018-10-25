using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using Exceptions;

namespace CoreApi
{
    public class UserManager : IUserManger
    {
        private UserCrudFactory _crudFactory { get; set; }

        public UserManager()
        {
            _crudFactory = new UserCrudFactory();
        }

        public ManagerActionResult<User> RegisterUser(User user)
        {
            try
            {
                var newUser = _crudFactory.Create<User>(user);

                if (newUser != null)
                {
                    return new ManagerActionResult<User>(newUser, ManagerActionStatus.Created);
                }
                else
                {
                    return new ManagerActionResult<User>(user, ManagerActionStatus.NothingModified);
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
                        throw sqlEx;
                }
                return new ManagerActionResult<User>(
                    null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface IUserManger
    {
        ManagerActionResult<User> RegisterUser(User user);
    }
}
