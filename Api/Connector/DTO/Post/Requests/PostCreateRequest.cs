using Forum_API.Cores.Validations;

namespace Forum_API.DTO.Post.Requests
{
    public class PostCreateRequest
    {
        [NotNull]
        [HtmlSecurity]
        public string Content { get; set; }

        [NotNull]
        public long TopicId { get; set; }
    }
}
