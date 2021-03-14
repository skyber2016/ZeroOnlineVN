using API.Cores.Validations;

namespace API.DTO.Card.Requests
{
    public class CardCreateRequest
    {
        [NotNull(Common.Message.NotNull, "loại thẻ")]
        [MaxLength(20, Common.Message.MaxLength, "loại thẻ")]
        public int Type { get; set; }
        [MaxLength(20, Common.Message.MaxLength, "seri")]
        [NotNull(Common.Message.NotNull, "seri")]
        public string Seri { get; set; }
        [MaxLength(20, Common.Message.MaxLength, "mã thẻ")]
        [NotNull(Common.Message.NotNull, "mã thẻ")]
        public string Code { get; set; }
        [MaxLength(20, Common.Message.MaxLength, "mệnh giá")]
        [NotNull(Common.Message.NotNull, "mệnh giá")]
        public int Value { get; set; }

        public int Status { get; set; }
    }
}
