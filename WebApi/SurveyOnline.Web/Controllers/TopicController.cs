using Entities_POJO;
using Microsoft.AspNet.Identity;
using PagedList;
using SurveyOnline.Web.Helper;
using SurveyOnline.Web.Services;
using SurveyOnline.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyOnline.Web.Controllers
{
    [Authorize]
    public class TopicController : SurveyOnlineController
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

        public async Task<ActionResult> TopicForm(string selectedCategories = null, string title = null)
        {
            var claims = User as ClaimsPrincipal;
            var categoryService = new CategoryService(claims.FindFirst("access_token").Value);
            var model = new ViewModelForTopicForm
            {
                Categories = await categoryService.GetCategoriesAsync()
            };

            if (selectedCategories != null) model.SelectedCategories = selectedCategories;

            if (title != null) model.Title = title;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterTopic(ViewModelForTopicForm model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("TopicForm", "Topic",
                new { model.SelectedCategories, model.Title });
            }

            var claims = User as ClaimsPrincipal;
            var userId = Guid.Parse(User.Identity.GetUserId());
            var topicService = new TopicService(claims.FindFirst("access_token").Value);
            List<Category> categories = null;

            var uploadedImage = new byte[model.Image.InputStream.Length];
            model.Image.InputStream.Read(uploadedImage, 0, uploadedImage.Length);

            if (model.SelectedCategories != null)
            {
                categories = new List<Category>();

                foreach (var categoryId in model.SelectedCategories.Split(','))
                {
                    if (int.TryParse(categoryId, out int id))
                    {
                        categories.Add(new Category { Id = id });
                    }
                }
            }

            var topicForRegistration = new TopicForRegistration
            {
                Title = model.Title,
                Categories = categories,
                UserId = userId,
                Description = " ",
                Picture = new PictureForEntity
                {
                    Extension = Path.GetExtension(model.Image.FileName),
                    Picture = uploadedImage,
                }
            };

            await topicService.RegisterTopicAsync(userId, topicForRegistration);

            return RedirectToAction("Index", "Topic");
        }
    }
}