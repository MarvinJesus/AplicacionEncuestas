using CoreApi;
using Exceptions;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class CategoryController : SurveyOnlineController
    {
        private ICategoryManager _categoryManager { get; set; }

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [HttpGet]
        public IHttpActionResult GetCategories()
        {
            try
            {
                return Ok(_categoryManager.GetCategories());
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }
    }
}
