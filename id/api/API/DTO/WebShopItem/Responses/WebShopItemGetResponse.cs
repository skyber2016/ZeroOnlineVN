using System.Text.Json.Serialization;

namespace API.DTO.WebShopItem.Responses
{
    public class WebShopItemGetResponse
    {
        public int Id { get; set; }

        public int Qty { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public int Price { get; set; }

        public string PriceStr
        {
            get
            {
                return this.Price.ToString("#,##0");
            }
        }

    }
}
