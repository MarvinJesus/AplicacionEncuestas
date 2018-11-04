using System;
using System.Collections.Generic;

namespace Entities_POJO
{
    public class SurveyForRegistration
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PictureForEntity Picture { get; set; }

        public Guid TopicId { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
