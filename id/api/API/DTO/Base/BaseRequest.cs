using System.ComponentModel.DataAnnotations;

namespace API.DTO.Base
{
    public class BaseRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
