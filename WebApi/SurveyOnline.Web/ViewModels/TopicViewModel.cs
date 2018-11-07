using Entities_POJO;
using PagedList;
using SurveyOnline.Web.Helper;
using System.Collections.Generic;

namespace SurveyOnline.Web.ViewModels
{
    public class TopicViewModel
    {
        public ICollection<Category> Categories { get; set; }
        public IPagedList<Topic> Topics { get; set; }
        public ICollection<Category> SelectedCategories { get; set; }
        public string SearchTeam { get; set; }
        public string CategoriesInList { get; set; }
        public PagingInfo PaginInfo { get; set; }
    }
}