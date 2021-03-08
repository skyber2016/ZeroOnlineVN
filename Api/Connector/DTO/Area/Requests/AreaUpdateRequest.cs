using Forum_API.DTO.Base;

namespace Forum_API.DTO.Area.Requests
{
    public class AreaUpdateRequest : UpdateRequest
    {
        public string Name { get; set; }
        public int Order { get; set; }
    }
}
