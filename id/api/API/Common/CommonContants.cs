using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common
{
    public static class CommonContants
    {
        public static Dictionary<int, string> Question
        {
            get
            {
                return new Dictionary<int, string>()
                {
                    [1] = "Tên con vật yêu thích?",
                    [2] = "Trường cấp 1 của bạn tên gì?",
                    [3] = "Người bạn yêu quý nhất?",
                    [4] = "Trò chơi bạn thích nhất?",
                    [5] = "Nơi để lại kỉ niệm khó quên nhất?",
                };
            }
        }

        public static Dictionary<int,string> CardType
        {
            get
            {
                return new Dictionary<int, string>
                {
                    [1] = "VIETTEL",
                    [2] = "MOBI",
                    [3] = "VINA",
                    [4] = "ZING",
                    [5] = "FPT",
                    [6] = "VTC"
                };
            }
        }

        public static Dictionary<int,string> CardResponse
        {
            get
            {
                return new Dictionary<int, string>
                {
                    [100] = "Giao dịch nạp thành công",
                    [101] = "Dữ liệu DataSign không đúng.",
                    [102] = "Mạng đang bảo trị hoặc sự cố",
                    [103] = "Tài khoản không đúng hoặc đang bị khóa",
                    [104] = "MerchantId không chính xác hoặc chưa kích hoạt",
                    [105] = "Hệ thống quá tải",
                    [106] = "Mệnh giá thẻ cào không được hỗ trợ",
                    [107] = "Thẻ trễ hoặc hệ thống đang gặp sự cố",
                    [108] = "Thông tin thẻ nạp không chính xác",
                    [109] = "Nạp thẻ thành công nhưng sai mệnh giá nên không nhận được tiền",
                    [110] = "Hệ thống quá tải",
                    [111] = "Sai định dạng thẻ cào",
                    [112] = "Nạp thẻ quá nhanh trong 1 phút",
                    [113] = "Nạp sai liên tiếp quá nhiều lần. Tạm khóa",
                    [114] = "Thẻ này đã nạp thành công vào hệ thống rồi.",
                    [0] = "Dữ liệu gửi lên không chính xác",
                };
            }
        }

        public static string GetCardResponse(int code)
        {
            if (CardResponse.ContainsKey(code))
            {
                return CardResponse[code];
            }
            return "Thẻ đang được xác định";
        }

        public static IDictionary<int,int> ActionMoney
        {
            get
            {
                return new Dictionary<int, int>();
            }
        }
        public static IDictionary<int,int> ActionRewardVip
        {
            get
            {
                return new Dictionary<int, int>
                {
                    [1] = 301,
                    [2] = 302,
                    [3] = 303,
                    [4] = 304,
                    [5] = 305,
                    [6] = 306
                };
            }
        }

    }
}
