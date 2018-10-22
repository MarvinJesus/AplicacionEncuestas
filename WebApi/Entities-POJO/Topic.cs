using System;
using System.Collections.Generic;

namespace Entities_POJO
{
    public class Topic : BaseEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public Guid UserId { get; set; }

        public ICollection<Question> Questions { get; set; }

        public Topic()
        {
        }

        public Topic(int id, string title, string description, string imagePath, Guid userId)
        {
            Id = id;
            Title = title;
            Description = description;
            ImagePath = imagePath;
            UserId = userId;
        }
    }
}