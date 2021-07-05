using System.ComponentModel.DataAnnotations;

namespace Entity.DTO.Base
{
    public class BaseRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
