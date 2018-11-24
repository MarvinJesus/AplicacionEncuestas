using SurveyOnline.Constants;
using System.Web;
using System.Web.Mvc;

namespace SurveyOnline.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return Redirect(SurveyOnlineConstants.SurveyOnlineRegisterPage);
        }

        [Authorize]
        public ActionResult Login()
        {
            return Redirect("/Profile");
        }

        [Authorize]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
    }
}