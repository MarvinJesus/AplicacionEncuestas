using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Factory
{
    public class SurveyFactory : EntityFactory
    {
        private string QUESTION_FIELD { get; set; }
        private List<string> MAIN_PROPERTIES { get; set; }

        public SurveyFactory()
        {
            var surveyDefault = new Survey();

            QUESTION_FIELD = nameof(surveyDefault.Questions);
            MAIN_PROPERTIES = new List<string>
            {
                nameof(surveyDefault.Id),
                nameof(surveyDefault.Title),
                nameof(surveyDefault.Description),
                nameof(surveyDefault.ImagePath)
            };
        }

        public object CreateDataShapeObject(Survey survey)
        {
            return CreateDataShapeObject(survey, MAIN_PROPERTIES);
        }

        public object CreateDataShapeObject(Survey survey, List<string> listOfFields)
        {
            return CreateDataShapeObject(survey, listOfFields, new List<string> { QUESTION_FIELD });
        }
    }
}
