namespace Entities_POJO
{
    public class Answer : BaseEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int QuestionId { get; set; }

        public Answer()
        {
        }

        public Answer(int id, string description, int questionId)
        {
            Id = id;
            Description = description;
            QuestionId = questionId;
        }
    }
}
