using Entities_POJO;
using System.Collections.Generic;

namespace SurveyOnline.Web.ViewModels
{
    public class ProfileViewModel
    {
        public Profile Profile { get; set; }
        public ICollection<Topic> Topics { get; set; }
    }
}