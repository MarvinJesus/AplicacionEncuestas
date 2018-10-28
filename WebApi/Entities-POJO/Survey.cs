using System;
using System.Collections.Generic;

namespace Entities_POJO
{
    public class Survey : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public Guid TopicId { get; set; }

        public ICollection<Question> Questions { get; set; }

        public Survey()
        {
        }

        public Survey(Guid id, string title, string description, string imagePath, Guid topicId)
        {
            Id = id;
            Title = title;
            Description = description;
            ImagePath = imagePath;
            TopicId = topicId;
        }
    }
}
