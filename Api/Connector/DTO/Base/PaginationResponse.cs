using System.Collections.Generic;

namespace Forum_API.DTO.Base
{
    public class PaginationResponse<T>
    {
        public long TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
