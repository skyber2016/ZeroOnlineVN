namespace Forum_API.DTO.Category.Requests
{
    public class CategoryCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long AreaId { get; set; }
    }
}
