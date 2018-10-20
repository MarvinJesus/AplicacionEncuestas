using System;

namespace Entities_POJO
{
    public class Profile : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Identification { get; set; }

        public string Email { get; set; }

        public string ImagePath { get; set; }

        public Profile()
        {

        }

        public Profile(Guid id, Guid userId, string name, string identification, string email, string imagePath)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Identification = identification;
            Email = email;
            ImagePath = imagePath;
        }
    }
}
