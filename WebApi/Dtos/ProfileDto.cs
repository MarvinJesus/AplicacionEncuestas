using System;

namespace Dtos
{
    public class ProfileDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Identification { get; set; }

        public string Email { get; set; }

        public string ImagePath { get; set; }
    }
}
