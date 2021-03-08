using Forum_API.DTO.Base;

namespace Forum_API.DTO.Category.Requests
{
    public class CategoryUpdateRequest : UpdateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
