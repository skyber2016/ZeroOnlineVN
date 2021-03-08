using Forum_API.DTO.Base;
using Forum_API.DTO.Topic.Responses;
using System;

namespace Forum_API.DTO.Category.Responses
{
    public class CategoryGetResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalTopics { get; set; }
        public BaseUserGetResponse CreatedByUser { get; set; }
        public TopicGetResponse LastTopic { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
