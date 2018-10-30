namespace Entities_POJO
{
    public class Category : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category()
        {
        }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
