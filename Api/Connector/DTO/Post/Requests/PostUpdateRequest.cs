using Forum_API.Cores.Validations;
using Forum_API.DTO.Base;

namespace Forum_API.DTO.Post.Requests
{
    public class PostUpdateRequest : UpdateRequest
    {
        [NotNull]
        [HtmlSecurity]
        public string Content { get; set; }
    }
}
