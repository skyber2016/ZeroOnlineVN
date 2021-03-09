using System.ComponentModel.DataAnnotations;

namespace API.DTO.Base
{
    public class UpdateRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
