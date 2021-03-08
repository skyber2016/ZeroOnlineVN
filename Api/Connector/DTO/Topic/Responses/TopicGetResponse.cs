using Forum_API.DTO.Base;
using Forum_API.Helpers;
using System;

namespace Forum_API.DTO.Topic.Responses
{
    public class TopicGetResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Views { get; set; }
        public bool IsEnabled { get; set; }
        public BaseUserGetResponse CreatedByUser { get; set; }
        public int Replies { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Slug { get; set; }
    }
}
