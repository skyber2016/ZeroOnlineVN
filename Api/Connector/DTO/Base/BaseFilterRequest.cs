namespace Forum_API.DTO.Base
{
    public class BaseFilterRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
