using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Factory
{
    public class TopicFactory : EntityFactory
    {
        private string CATEGORY_FIELD { get; set; }
        private List<string> MAIN_PROPERTIES { get; set; }

        public TopicFactory()
        {
            var topicDefault = new Topic();

            CATEGORY_FIELD = nameof(topicDefault.Categories);
            MAIN_PROPERTIES = new List<string>
            {
                nameof(topicDefault.Id),
                nameof(topicDefault.Title),
                nameof(topicDefault.Description),
                nameof(topicDefault.ImagePath)
            };
        }

        public object CreateDataShapeObject(Topic topic)
        {
            return CreateDataShapeObject(topic, MAIN_PROPERTIES);
        }

        public object CreateDataShapeObject(Topic topic, List<string> listOfFields)
        {
            return CreateDataShapeObject(topic, listOfFields, new List<string> { CATEGORY_FIELD });
        }
    }
}
