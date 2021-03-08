using System.ComponentModel.DataAnnotations;

namespace Forum_API.DTO.Base
{
    public class BaseRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
