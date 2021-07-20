namespace API.DTO.RewardShopItem.Responses
{
    public class RewardShopItemUpdateRequest
    {
        public int Id { get; set; }
        public int ActionId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public int Group { get; set; }
    }
}
