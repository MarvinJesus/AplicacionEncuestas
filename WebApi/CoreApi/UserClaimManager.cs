using CoreApi.ActionResult;
using DataAccess.Crud;
using Entities_POJO;
using System.Collections.Generic;

namespace CoreApi
{
    public class UserClaimManager : IUserClaimManager
    {
        private UserClaimCrudFactory _crudFactory { get; set; }

        public UserClaimManager()
        {
            _crudFactory = new UserClaimCrudFactory();
        }

        public ManagerActionResult<UserClaim> RegisterUserClaim(UserClaim userClaim)
        {
            try
            {
                if (userClaim == null)
                    return new ManagerActionResult<UserClaim>(null, ManagerActionStatus.Error);

                var newUserClaim = _crudFactory.Create<UserClaim>(userClaim);

                if (newUserClaim != null)
                    return new ManagerActionResult<UserClaim>(newUserClaim, ManagerActionStatus.Created);

                return new ManagerActionResult<UserClaim>(userClaim, ManagerActionStatus.Error);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ManagerActionResult<ICollection<UserClaim>> RegisterUserClaims(ICollection<UserClaim> userClaims)
        {
            try
            {
                ICollection<UserClaim> ListUserClaims = new List<UserClaim>();

                if (userClaims == null)
                    return new ManagerActionResult<ICollection<UserClaim>>(null, ManagerActionStatus.Error);

                foreach (var userClaim in userClaims)
                {
                    var result = RegisterUserClaim(userClaim);

                    if (result.Status == ManagerActionStatus.Created)
                    {
                        ListUserClaims.Add(result.Entity);
                    }
                }

                return new ManagerActionResult<ICollection<UserClaim>>(ListUserClaims, ManagerActionStatus.Created);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ICollection<UserClaim> GetUserClaims(Profile profile)
        {
            try
            {
                return _crudFactory.RetrieveUserClaimByUser(new UserClaim { UserId = profile.UserId });
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    public interface IUserClaimManager
    {
        ManagerActionResult<UserClaim> RegisterUserClaim(UserClaim userClaim);
        ManagerActionResult<ICollection<UserClaim>> RegisterUserClaims(ICollection<UserClaim> userClaims);
        ICollection<UserClaim> GetUserClaims(Profile profile);
    }
}
