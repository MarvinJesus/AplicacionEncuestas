using Entities_POJO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SurveyOnline.Web.ViewModels
{
    public class ViewModelForTopicForm
    {
        public ICollection<Category> Categories { get; set; }
        public string SelectedCategories { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public HttpPostedFileBase Image { get; set; }
    }
}