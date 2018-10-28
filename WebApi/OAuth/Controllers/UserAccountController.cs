using CoreApi;
using Entities_POJO;
using Exceptions;
using IdentityServer3.Core;
using OAuth.Helper;
using OAuth.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OAuth.Controllers
{
    public class UserAccountController : Controller
    {
        public ActionResult Index()
        {
            return View(new ViewModelForUserRegistration());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ViewModelForUserRegistration model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                if (model.Password != null && !model.Password.Equals(model.PasswordConfirmation))
                {
                    model.ErrorMessage = "Confirmación de contraseña no coincide con el campo contraseña";
                    return View(model);
                }

                if (!model.TermAndConditions)
                {
                    model.ErrorMessage = "Aceptar los términos y condiciones";
                    return View(model);
                }

                var result = new ProfileManager().RegisterProfile(ProfileFactory.GetProfileForRegistration(model));

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                {
                    model.ErrorMessage = result.Exception?.AppMessage?.Message;
                    return View(model);
                }

                new UserClaimManager().RegisterUserClaims(GetUserClaims(result.Entity, true));

                return Content("User Registrated");//Change it, It must redirect to the login page
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return Redirect("/Error");
            }
        }

        public ICollection<UserClaim> GetUserClaims(Profile profile, bool includeUserRoles)
        {

            ICollection<UserClaim> userClaims = new List<UserClaim>
            {
                new UserClaim(profile.UserId, profile.Name, Constants.ClaimTypes.Name),
                new UserClaim(profile.UserId, profile.Email, Constants.ClaimTypes.Email),
                new UserClaim(profile.UserId, profile.ImagePath, Constants.ClaimTypes.Picture),
                new UserClaim(profile.UserId, profile.Identification, Constants.ClaimTypes.Id)
            };

            if (!includeUserRoles) return userClaims;

            var userRoles = new ProfileManager().GetProfileRoles(profile.UserId);

            if (userRoles.Count > 0)
            {
                foreach (var role in userRoles)
                {
                    userClaims.Add(new UserClaim(profile.UserId, role.Name, Constants.ClaimTypes.Role));
                }
            }

            return userClaims;
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}