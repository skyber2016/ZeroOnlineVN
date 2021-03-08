using Forum_API.Cores.Validations;

namespace Forum_API.DTO.Base
{
    public class UpdateRequest
    {
        [NotNull]
        public long Id { get; set; }
    }
}
