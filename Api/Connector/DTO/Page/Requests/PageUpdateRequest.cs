using Forum_API.DTO.Base;

namespace Forum_API.DTO.Page.Requests
{
    public class PageUpdateRequest : UpdateRequest
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

    }
}
