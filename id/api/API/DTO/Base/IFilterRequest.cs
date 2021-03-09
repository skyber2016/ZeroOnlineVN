namespace API.DTO.Base
{
    public class PaginationRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationRequest()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        
    }
}
