namespace Entities_POJO
{
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Role()
        {
        }

        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
