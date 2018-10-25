using System;

namespace Entities_POJO
{
    public class UserClaim : BaseEntity
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public UserClaim()
        {
        }

        public UserClaim(Guid userId, string value, string type) : this(0, userId, value, type)
        {
        }

        public UserClaim(int id, Guid userId, string value, string type)
        {
            Id = id;
            UserId = userId;
            Value = value;
            Type = type;
        }
    }
}
