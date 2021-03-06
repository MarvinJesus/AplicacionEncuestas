﻿using CoreApi;
using Exceptions;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class CategoriesController : SurveyOnlineController
    {
        private ICategoryManager _categoryManager { get; set; }

        public CategoriesController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("categories")]
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
