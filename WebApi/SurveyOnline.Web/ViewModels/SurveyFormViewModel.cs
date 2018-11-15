using Entities_POJO;
using System.Collections.Generic;

namespace SurveyOnline.Web.ViewModels
{
    public class SurveyFormViewModel
    {
        public ICollection<Topic> Topics { get; set; }
    }
}