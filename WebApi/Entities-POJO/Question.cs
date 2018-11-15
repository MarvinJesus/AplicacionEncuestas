using System;
using System.Collections.Generic;

namespace Entities_POJO
{
    public class Question : BaseEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public Guid SurveyId { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public Question()
        {
        }

        public Question(int id, string description, Guid surveyId)
        {
            Id = id;
            Description = description;
            SurveyId = surveyId;
        }

    }
}
