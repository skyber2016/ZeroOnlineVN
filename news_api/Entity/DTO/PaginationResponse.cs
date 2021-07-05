using System.Collections.Generic;

namespace Entity.DTO.Base
{
    public class PaginationResponse<T>
    {
        public int TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
