using Microsoft.AspNetCore.Http;

namespace API.DTO.WebShopItem.Requests
{
    public class WebShopItemCreateRequest
    {
        public int Id { get; set; }

        public int Qty { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Image { get; set; }
    }
}
