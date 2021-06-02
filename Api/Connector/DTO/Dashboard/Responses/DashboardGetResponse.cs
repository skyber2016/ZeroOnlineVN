using Forum_API.DTO.Area.Responses;
using Forum_API.DTO.ShoutBox.Responses;
using Forum_API.DTO.Topic.Responses;
using Forum_API.DTO.User.Responses;
using System.Collections.Generic;

namespace Forum_API.DTO.Dashboard.Responses
{
    public class DashboardGetResponse
    {
        public IEnumerable<ShoutBoxGetResponse> ShoutBoxs { get; set; }
        public IEnumerable<AreaGetResponse> Areas { get; set; }
        public IEnumerable<TopicGetResponse> LastTopics { get; set; }
        public long TotalPosts { get; set; }
        public long TotalMembers { get; set; }
        public UserGetResponse NewestMember { get; set; }
        public long TotalTopic { get; set; }
    }
}
