using Forum_API.Cores.Validations;

namespace Forum_API.DTO.Topic.Requests
{
    public class TopicCreateRequest
    {
        [NotNull]
        [MaxLength(200)]
        public string Name { get; set; }
        [NotNull]
        public long CategoryId { get; set; }
    }
}
