using Forum_API.DTO.Topic.Responses;

namespace Forum_API.DTO.Category.Responses
{
    public class CategoryGetByIdResponse
    {
        public TopicGetResponse Announcements { get; set; }
        public TopicGetResponse Topics { get; set; }
    }
}
