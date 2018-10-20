using System;

namespace Entities_POJO
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }

        public User()
        {
        }

        public User(Guid userId, string username, byte[] password, byte[] salt)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Salt = salt;
        }
    }
}
