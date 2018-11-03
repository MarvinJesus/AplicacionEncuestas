using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Factory
{
    public class QuestionFactory : EntityFactory
    {
        private string ANSWER_FIELD { get; set; }
        private List<string> MAIN_PROPERTIES { get; set; }

        public QuestionFactory()
        {
            var questionDefault = new Question();

            ANSWER_FIELD = nameof(questionDefault.Answers);
            MAIN_PROPERTIES = new List<string>
            {
                nameof(questionDefault.Id),
                nameof(questionDefault.Description),
                nameof(questionDefault.SurveyId)
            };
        }

        public object CreateDataShapeObject(Question question)
        {
            return CreateDataShapeObject(question, MAIN_PROPERTIES);
        }

        public object CreateDataShapeObject(Question question, List<string> listOfFields)
        {
            return CreateDataShapeObject(question, listOfFields, new List<string> { ANSWER_FIELD });
        }
    }
}
