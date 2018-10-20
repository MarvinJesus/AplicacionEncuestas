using System.Collections.Generic;

namespace Entities_POJO
{
    public class Question : BaseEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int TopicId { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public Question()
        {
        }

        public Question(int id, string description, int topicId)
        {
            Id = id;
            Description = description;
            TopicId = topicId;
        }

    }
}
