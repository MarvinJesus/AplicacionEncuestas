using Entities_POJO;
using PagedList;
using SurveyOnline.Web.Helper;
using SurveyOnline.Web.Services;
using SurveyOnline.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyOnline.Web.Controllers
{
    public class TopicController : Controller
    {
        [Authorize]
        public async Task<ActionResult> Index(string query = null, string filters = null, int? page = 1)
        {
            var claims = User as ClaimsPrincipal;
            var viewModel = new TopicViewModel();
            IEnumerable<string> selectedFilters = null;

            var paramList = new Dictionary<string, string>
            {
                { "sort", "id" },
                { "fields","id,title,imagepath,totalSurvey"},
                { "page", Convert.ToString(page)},
                { "pageSize", "5"}
            };

            if (query != null)
            {
                paramList.Add("search", query);
                viewModel.SearchTeam = query;
            }

            if (filters != null)
            {
                paramList.Add("filters", filters);
                selectedFilters = filters.Split(',').ToList();
                viewModel.CategoriesInList = filters;

            }

            var categoryService = new CategoryService(claims.FindFirst("access_token").Value);
            var topicService = new TopicService(claims.FindFirst("access_token").Value);

            viewModel.Categories = await categoryService.GetCategoriesAsync();
            viewModel.SelectedCategories = viewModel.Categories.ReturnEquals(selectedFilters);
            viewModel.Categories = viewModel.Categories.ReturnDiferents(selectedFilters);

            var topics = await topicService.GetTopicsAsync(paramList);
            viewModel.PaginInfo = topicService.GetPagingInfo();

            var pageTopicList = new StaticPagedList<Topic>(topics, viewModel.PaginInfo.CurrentPage, viewModel.PaginInfo.PageSize,
                viewModel.PaginInfo.TotalCount);

            viewModel.Topics = pageTopicList;


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Search(TopicViewModel viewModel)
        {
            return RedirectToAction("Index", "Topic", new
            {
                query = viewModel.SearchTeam,
                filters = viewModel.CategoriesInList
            });
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        public ActionResult Mine()
        {
            return View();
        }
    }
}