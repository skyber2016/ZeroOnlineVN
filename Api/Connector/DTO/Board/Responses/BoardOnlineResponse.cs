using Forum_API.DTO.Base;
using System.Collections.Generic;

namespace Forum_API.DTO.Board.Responses
{
    public class BoardOnlineResponse
    {
        public IEnumerable<BaseUserGetResponse> Users { get; set; }
        public int Guest { get; set; }
    }
}
