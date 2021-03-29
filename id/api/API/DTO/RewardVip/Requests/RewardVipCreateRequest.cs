using API.Cores.Validations;

namespace API.DTO.RewardVip.Requests
{
    public class RewardVipCreateRequest
    {
        [NotNull("Vui lòng chọn mốc vip")]
        public int Id { get; set; }
    }
}
