namespace Forum_API.DTO.Base
{
    public class BaseGetAllResponse<T>
    {
        public int TotalRecords { get; set; }
        public T Data { get; set; }
    }
}
