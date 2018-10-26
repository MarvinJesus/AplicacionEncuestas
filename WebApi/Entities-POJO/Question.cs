using System;
using System.Collections.Generic;

namespace Entities_POJO
{
    public class Question : BaseEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public Guid TopicId { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public Question()
        {
        }

        public Question(int id, string description, Guid topicId)
        {
            Id = id;
            Description = description;
            TopicId = topicId;
        }

    }
}
