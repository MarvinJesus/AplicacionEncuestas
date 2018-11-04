using System;
using System.Collections.Generic;

namespace Entities_POJO
{
    public class TopicForRegistration
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public PictureForEntity Picture { get; set; }

        public Guid UserId { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
