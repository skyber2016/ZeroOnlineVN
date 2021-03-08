using Forum_API.DTO.Category.Responses;
using System.Collections.Generic;

namespace Forum_API.DTO.Area.Responses
{
    public class AreaGetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public IEnumerable<CategoryGetResponse> Categories { get; set; }
    }
}
