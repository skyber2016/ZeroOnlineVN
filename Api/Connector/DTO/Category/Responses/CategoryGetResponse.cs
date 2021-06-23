using Forum_API.DTO.Base;
using Forum_API.DTO.Topic.Responses;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Forum_API.DTO.Category.Responses
{
    public class CategoryGetResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalTopics { get; set; }
        public long Views { get; set; }
        public BaseUserGetResponse CreatedByUser { get; set; }
        public TopicGetResponse LastTopic { get; set; }
        public DateTime CreatedDate { get; set; }
        [JsonPropertyName("subCategories")]
        public IEnumerable<CategoryGetResponse> Childrens { get; set; }
    }
}
