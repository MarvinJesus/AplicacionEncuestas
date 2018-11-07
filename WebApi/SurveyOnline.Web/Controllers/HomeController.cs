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
            return Redirect("/Topic");
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
    }
}