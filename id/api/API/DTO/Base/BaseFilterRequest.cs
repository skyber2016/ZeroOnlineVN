namespace API.DTO.Base
{
    public class BaseFilterRequest
    {
        public string KeySearch { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public BaseFilterRequest()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.KeySearch = string.Empty;
        }
    }
}
