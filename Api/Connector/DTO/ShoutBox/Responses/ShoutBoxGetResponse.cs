using Forum_API.DTO.Base;
using System;

namespace Forum_API.DTO.ShoutBox.Responses
{
    public class ShoutBoxGetResponse
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public BaseUserGetResponse CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
